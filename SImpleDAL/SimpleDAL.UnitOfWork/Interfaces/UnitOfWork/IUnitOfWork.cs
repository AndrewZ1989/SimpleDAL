using System;
using System.Data;

namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// ��������� UoW
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Properties

        /// <summary>
        /// ������� ����, ��� ������� ������� ������ ��� ������������ ����� ������-�����������
        /// </summary>
        bool IsInTransaction { get; }

        #endregion

        #region methods

        /// <summary>
        /// ������ ������-����������
        /// </summary>
        ITransaction BeginTransaction();
        ITransaction BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// �������� ��� ����������� ��������� � ��������� ������
        /// </summary>
        void Flush();
        void TransactionalFlush();
        void TransactionalFlush(IsolationLevel isolationLevel);

        #endregion
    }
}