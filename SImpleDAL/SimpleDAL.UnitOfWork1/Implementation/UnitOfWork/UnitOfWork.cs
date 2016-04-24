using System;
using SimpleDAL.UnitOfWork.Storage;

namespace SimpleDAL.UnitOfWork
{
    public static class UnitOfWork
    {
        #region Const

        private const string currentUnitOfWorkKey = "CurrentUnitOfWorkSession.Key";

        #endregion

        #region Properties

        public static bool HasStarted
        {
            get { return Store.Local.Contains(currentUnitOfWorkKey); }
        }

        public static IUnitOfWork Current
        {
            get
            {
                if (!HasStarted) return null;
                return Store.Local.Get<IUnitOfWork>(currentUnitOfWorkKey);
            }
            set
            {
                if (value == null) Store.Local.Remove(currentUnitOfWorkKey);
                else
                    Store.Local.Set(currentUnitOfWorkKey, value);
            }
        }

        #endregion

        #region methods

        public static void Finish(bool flush)
        {
            if (!HasStarted)
            {
                throw new InvalidOperationException("It is not possible to complete a unit of work that is not running.");
            }

            if (flush) Current.TransactionalFlush();

            Current.Dispose();
            Current = null;
        }

        #endregion
    }
}