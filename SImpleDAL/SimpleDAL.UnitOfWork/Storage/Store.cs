using System.Collections;
using System.Web;

namespace SimpleDAL.UnitOfWork.Storage
{
    public abstract class Store
    {
        #region fields

        protected static readonly object AppStorageLock = new object();
        protected static readonly object LocalStorageLock = new object();
        protected static readonly object SessionStorageLock = new object();

        private static AppStorage _appStorage;
        private static LocalStorage _localStorage;
        #endregion

        #region properties

        public static bool IsWebApplication
        {
            get { return HttpContext.Current != null; }
        }

        public static Store Local
        {
            get
            {
                if (_localStorage == null)
                {
                    lock (LocalStorageLock)
                    {
                        if (_localStorage == null)
                            _localStorage = new LocalStorage();
                    }
                }
                return _localStorage;
            }
        }

        public static Store Application
        {
            get
            {
                if (_appStorage == null)
                {
                    lock (AppStorageLock)
                    {
                        if (_appStorage == null)
                            _appStorage = new AppStorage();
                    }
                }
                return _appStorage;
            }
        }

        #endregion

        #region methods

        public T Get<T>(object key)
        {
            if (UseLocking)
            {
                lock (LockInstance)
                    return (T) GetInternalHashtable()[key];
            }
            return (T) GetInternalHashtable()[key];
        }

        public void Set<T>(object key, T value)
        {
            if (UseLocking)
            {
                lock (LockInstance)
                    GetInternalHashtable()[key] = value;
            }
            else
                GetInternalHashtable()[key] = value;
        }

        public bool Contains(object key)
        {
            if (UseLocking)
            {
                lock (LockInstance)
                    return GetInternalHashtable().ContainsKey(key);
            }
            return GetInternalHashtable().ContainsKey(key);
        }

        public void Remove(object key)
        {
            if (UseLocking)
            {
                lock (LockInstance)
                    GetInternalHashtable().Remove(key);
            }
            else
                GetInternalHashtable().Remove(key);
        }

        public void Clear()
        {
            if (UseLocking)
            {
                lock (LockInstance)
                    GetInternalHashtable().Clear();
            }
            else
                GetInternalHashtable().Clear();
        }

        #endregion

        #region abstract members

        protected abstract bool UseLocking { get; }
        protected abstract object LockInstance { get; }
        protected abstract Hashtable GetInternalHashtable();

        #endregion
    }
}