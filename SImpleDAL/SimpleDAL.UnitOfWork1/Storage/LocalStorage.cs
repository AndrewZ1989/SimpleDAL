using System;
using System.Collections;
using System.Web;

namespace SimpleDAL.UnitOfWork.Storage
{
    public class LocalStorage : Store
    {
        #region fields
        [ThreadStatic] private static Hashtable _internalStorage;
        #endregion

        #region properties

        protected override bool UseLocking
        {
            get { return false; }
        }

        protected override object LockInstance
        {
            get { return null; }
        }

        #endregion

        #region methods

        protected override Hashtable GetInternalHashtable()
        {
            if (IsWebApplication)
            {
                var internalStorage = HttpContext.Current.Items[typeof (LocalStorage).FullName] as Hashtable;
                if (internalStorage == null)
                    HttpContext.Current.Items[typeof (LocalStorage).FullName] = internalStorage = new Hashtable();
                return internalStorage;
            }
            return _internalStorage ?? (_internalStorage = new Hashtable());
        }

        #endregion
    }
}