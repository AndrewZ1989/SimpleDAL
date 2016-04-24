using SimpleDAL.UnitOfWork.Storage;
using SimpleDAL.Utility.Extensions.EnumExtensions;
using System;
using System.Collections.Generic;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

namespace SimpleDAL.UnitOfWork
{
    public class UnitOfWorkScope : IUnitOfWorkScope
	{
		#region .ctor

		public UnitOfWorkScope( IUnitOfWorkFactory factory )
			: this( factory, GetScopeIsolationLevel(), UoWScopeOptions.UseCompatible ) { }

		public UnitOfWorkScope( IUnitOfWorkFactory factory, UoWScopeOptions options )
			: this( factory, GetScopeIsolationLevel(), options ) { }


		public UnitOfWorkScope( IUnitOfWorkFactory factory, IsolationLevel isolationLevel )
			: this( factory, isolationLevel, UoWScopeOptions.UseCompatible ) { }

		public UnitOfWorkScope( IUnitOfWorkFactory factory, IsolationLevel isolationLevel, UoWScopeOptions scopeOptions )
		{
			_useCompatibleEnabled = scopeOptions.Contains( UoWScopeOptions.UseCompatible );
			_disposed = false;
			_factory = factory;
			_transactionOptions = scopeOptions;
			_autoComplete = ( scopeOptions & UoWScopeOptions.AutoComplete ) == UoWScopeOptions.AutoComplete;
			_currentTransaction = UnitOfWorkScopeTransaction.GetTransactionForScope( factory, this, isolationLevel, scopeOptions );
			RegisterScope( this );
		}

        #endregion

        #region Fields

        private static readonly string UnitOfWorkScopeStackKey = typeof(UnitOfWorkScope).FullName + ".RunningScopeStack";
        private UnitOfWorkScopeTransaction _currentTransaction;
        private bool _disposed;
        private readonly bool _autoComplete;
        private readonly IUnitOfWorkFactory _factory;
        private readonly bool _useCompatibleEnabled;
        private readonly UoWScopeOptions _transactionOptions;

        #endregion

        #region Properties

        public bool HasStarted
		{
			get
			{
				if ( !Store.Local.Contains( UnitOfWorkScopeStackKey ) )
					return false;

				return RunningScopes.Count > 0;
			}
		}

		private Stack<UnitOfWorkScope> RunningScopes
		{
			get
			{
				if ( !Store.Local.Contains( UnitOfWorkScopeStackKey ) )
					Store.Local.Set( UnitOfWorkScopeStackKey, new Stack<UnitOfWorkScope>() );

				return Store.Local.Get<Stack<UnitOfWorkScope>>( UnitOfWorkScopeStackKey );
			}
		}

		public IUnitOfWork RelUnitOfWork
		{
			get
			{
				return _currentTransaction.UnitOfWork;
			}
		}

		public bool UseCompatibleEnabled
		{
			get
			{
				return _useCompatibleEnabled;
			}
		}

		#endregion

		#region Methods

		#region Static

		private static IsolationLevel GetScopeIsolationLevel()
		{
			return Transaction.Current == null ? IsolationLevel.ReadCommitted : MapToSystemDataIsolationLevel( Transaction.Current.IsolationLevel );
		}

		private static IsolationLevel MapToSystemDataIsolationLevel( System.Transactions.IsolationLevel isolationLevel )
		{
			switch ( isolationLevel )
			{
				case System.Transactions.IsolationLevel.Chaos:
					return IsolationLevel.Chaos;
				case System.Transactions.IsolationLevel.ReadCommitted:
					return IsolationLevel.ReadCommitted;
				case System.Transactions.IsolationLevel.ReadUncommitted:
					return IsolationLevel.ReadUncommitted;
				case System.Transactions.IsolationLevel.RepeatableRead:
					return IsolationLevel.RepeatableRead;
				case System.Transactions.IsolationLevel.Serializable:
					return IsolationLevel.Serializable;
				case System.Transactions.IsolationLevel.Snapshot:
					return IsolationLevel.Snapshot;
				case System.Transactions.IsolationLevel.Unspecified:
					return IsolationLevel.Unspecified;
				default:
					return IsolationLevel.ReadCommitted;
			}
		}

		#endregion

		#region Public

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		public void Commit()
		{
			if ( _disposed )
			{
				throw new ObjectDisposedException( "Cannot commit a disposed scope." );
			}
			_currentTransaction.Commit( this );
			_currentTransaction = null;
		}

		#endregion

		private void Dispose( bool disposing )
		{
			if ( !disposing ) return;
			if ( _disposed ) return;

			if ( _currentTransaction != null )
			{
				if ( _autoComplete )
				{
                    #region Try commit
                    try
                    {
                        _currentTransaction.Commit(this);
                    }
                    catch
                    {
                        _currentTransaction.Rollback(this);
                        _currentTransaction = null;
                        UnRegisterScope(this);
                        _disposed = true;
                        throw;
                    } 
                    #endregion
                }
				else
					_currentTransaction.Rollback( this );

				_currentTransaction = null;
			}

			UnRegisterScope( this );

			_disposed = true;
		}

		private void RegisterScope( UnitOfWorkScope scope )
		{
			UnitOfWork.Current = scope.RelUnitOfWork;
			RunningScopes.Push( scope );
		}

		private void UnRegisterScope( UnitOfWorkScope scope )
		{
			if ( RunningScopes.Peek() != scope )
			{
				throw new InvalidOperationException( "Cannot un-register scope." );
			}

			RunningScopes.Pop();

			if ( RunningScopes.Count > 0 )
			{
				UnitOfWorkScope currentScope = RunningScopes.Peek();
				UnitOfWork.Current = currentScope.RelUnitOfWork;
			}
			else
				UnitOfWork.Current = null;
		}

		#endregion
	}
}