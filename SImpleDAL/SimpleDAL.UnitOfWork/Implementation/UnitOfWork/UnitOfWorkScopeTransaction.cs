using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SimpleDAL.UnitOfWork.Storage;

namespace SimpleDAL.UnitOfWork
{
    public class UnitOfWorkScopeTransaction : IDisposable
    {
        #region .ctor

        public UnitOfWorkScopeTransaction(IUnitOfWorkFactory unitOfWorkFactory, IsolationLevel isolationLevel)
        {
            _transactionID = new Guid();
            _transactionRolledback = false;
            _disposed = false;

            _unitOfWork = unitOfWorkFactory.Create();
            _runningTransaction = _unitOfWork.BeginTransaction(isolationLevel);

            _isolationLevel = isolationLevel;
            _attachedScopes = new Stack<UnitOfWorkScope>();
        }

        #endregion

        #region Fields

        private readonly Stack<UnitOfWorkScope> _attachedScopes;
        private readonly IsolationLevel _isolationLevel;
        private readonly ITransaction _runningTransaction;
        private readonly Guid _transactionID;
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed;
        private bool _transactionRolledback;

        #endregion

        #region Properties

        /// <summary>
        /// Уникальный идентификатор транзакции
        /// </summary>
        public Guid TransactionID
        {
            get { return _transactionID; }
        }

        /// <summary>
        /// Уровень изоляции транзакции
        /// </summary>
        public IsolationLevel IsolationLevel
        {
            get { return _isolationLevel; }
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        private static LinkedList<UnitOfWorkScopeTransaction> CurrentTransactions
        {
            get
            {
                var key = typeof(UnitOfWorkScopeTransaction).FullName;
                if (!Store.Local.Contains(key))
                    Store.Local.Set(key, new LinkedList<UnitOfWorkScopeTransaction>());

                return Store.Local.Get<LinkedList<UnitOfWorkScopeTransaction>>(key);
            }
        }

        #endregion

        #region Methods

        #region Static

        public static UnitOfWorkScopeTransaction GetTransactionForScope(IUnitOfWorkFactory factory, UnitOfWorkScope scope, IsolationLevel isolationLevel)
        {
            return GetTransactionForScope(factory, scope, isolationLevel, UoWScopeOptions.UseCompatible);
        }

        public static UnitOfWorkScopeTransaction GetTransactionForScope(IUnitOfWorkFactory factory, UnitOfWorkScope scope, IsolationLevel isolationLevel, UoWScopeOptions options)
        {
            var useCompatibleTx = (options & UoWScopeOptions.UseCompatible) == UoWScopeOptions.UseCompatible;
            var createNewTx = (options & UoWScopeOptions.CreateNew) == UoWScopeOptions.CreateNew;

            //вот нельзя одновременно создавать новую транзакцию и использовать существующую
            if (useCompatibleTx && createNewTx)
            {
                throw new InvalidOperationException("Несовместимые опции запуска транзакции");
            }

            if (options == UoWScopeOptions.UseCompatible)
            {
                var transaction = (from t in CurrentTransactions where t.IsolationLevel == isolationLevel select t).FirstOrDefault();
                if (transaction != null)
                {
                    transaction.AttachScope(scope);
                    return transaction;
                }
            }

            var newTransaction = new UnitOfWorkScopeTransaction(factory, isolationLevel);
            newTransaction.AttachScope(scope);
            CurrentTransactions.AddFirst(newTransaction);
            return newTransaction;
        }

        #endregion

        #region Public

        public void Commit(UnitOfWorkScope scope)
        {
            #region Verify

            if (_disposed)
            {
                throw new ObjectDisposedException("Transaction disposed.");
            }
            if (_transactionRolledback)
            {
                throw new InvalidOperationException("A child scope or current scope has already rolled back the transaction.");
            }

            if (_attachedScopes.Peek() != scope)
            {
                throw new InvalidOperationException("Commit can only be called by the current UnitOfWorkScope instance.");
            }

            #endregion

            var currentScope = _attachedScopes.Pop();
            if (_attachedScopes.Count != 0)
                return;

            try
            {
                _unitOfWork.Flush();
                _runningTransaction.Commit();
                _runningTransaction.Dispose();
                _unitOfWork.Dispose();
                CurrentTransactions.Remove(this);
            }
            catch (Exception)
            {
                _attachedScopes.Push(currentScope);
                throw;
            }
        }

        public void Rollback(UnitOfWorkScope scope)
        {
            #region Verify

            if (_disposed)
            {
                throw new ObjectDisposedException("Cannot rollback a disposed transaction.");
            }

            if (_attachedScopes.Peek() != scope)
            {
                throw new InvalidOperationException("Rollback can only be called by the current UnitOfWorkScope instance.");
            }

            #endregion

            _attachedScopes.Pop();

            //если у текущей транзакции не осталось прикреплённых транзакционных областей - осовободим её
            //иначе не мешаем внешним областям делать откат/подтверждение при наличии опции UseCompatible
            if (_attachedScopes.Count == 0)
            {
                _runningTransaction.Rollback();
                _runningTransaction.Dispose();
                _unitOfWork.Dispose();
                CurrentTransactions.Remove(this);
            }
            else
                if (!scope.UseCompatibleEnabled) _transactionRolledback = true;

        }

        #endregion

        private void AttachScope(UnitOfWorkScope scope)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Transaction is disposed.");
            }

            _attachedScopes.Push(scope);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}