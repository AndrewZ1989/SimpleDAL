using System.Collections;
using System.Web;

namespace SimpleDAL.UnitOfWork.Storage
{
    public class AppStorage : Store
    {
        #region fields

        private static Hashtable _internalStorage;

        #endregion

        #region properties

        protected override bool UseLocking
        {
            get { return true; }
        }

        protected override object LockInstance
        {
            get { return AppStorageLock; }
        }

        #endregion

        #region methods

        protected override Hashtable GetInternalHashtable()
        {
            if (IsWebApplication)
            {
                var internalHashtable = HttpContext.Current.Application[typeof (AppStorage).FullName] as Hashtable;
                if (internalHashtable == null)
                {
                    lock (AppStorageLock)
                    {
                        internalHashtable = HttpContext.Current.Application[typeof(AppStorage).FullName] as Hashtable;
                        if (internalHashtable == null)
                            HttpContext.Current.Application[typeof(AppStorage).FullName] =
                                internalHashtable = new Hashtable();
                    }
                }
                return internalHashtable;
            }

            if (_internalStorage == null)
            {
                lock (AppStorageLock)
                {
                    if (_internalStorage == null)
                        _internalStorage = new Hashtable();
                }
            }
            return _internalStorage;
        }

        #endregion
    }
}