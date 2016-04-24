using System;
using System.Data;

namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// Интерфейс UoW
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        /// <summary>
        /// Признак того, что текущая единица работы уже используется некой бизнес-транзакцией
        /// </summary>
        bool IsInTransaction { get; }

        #endregion

        #region methods

        /// <summary>
        /// Начать бизнес-транзакцию
        /// </summary>
        ITransaction BeginTransaction();
        ITransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Сбросить все накопленные изменения в хранилище данных
        /// </summary>
        void Flush();
        void TransactionalFlush();
        void TransactionalFlush(IsolationLevel isolationLevel);

        #endregion
    }
}