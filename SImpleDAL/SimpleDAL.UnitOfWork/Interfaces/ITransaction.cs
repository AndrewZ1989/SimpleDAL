using System;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Интерфейс бизнес-транзакции
	/// </summary>
	public interface ITransaction : IDisposable
	{
        #region Events

        event EventHandler TransactionCommitted;

        event EventHandler TransactionRolledback;

        #endregion

        #region Methods

        /// <summary>
        /// Подтвердить транзакцию
        /// </summary>
        void Commit();

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        void Rollback(); 

        #endregion
    }
}