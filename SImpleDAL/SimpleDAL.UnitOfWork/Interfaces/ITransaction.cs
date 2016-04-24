using System;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// ��������� ������-����������
	/// </summary>
	public interface ITransaction : IDisposable
	{
        #region Events

        event EventHandler TransactionCommitted;

        event EventHandler TransactionRolledback;

        #endregion

        #region Methods

        /// <summary>
        /// ����������� ����������
        /// </summary>
        void Commit();

        /// <summary>
        /// �������� ����������
        /// </summary>
        void Rollback(); 

        #endregion
    }
}