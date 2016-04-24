using SimpleDAL.Utility;
using System;

namespace SimpleDAL.Repository.DAL.Entities
{
    public class BaseEntity : IEntity
    {
        #region Fields

        protected Guid _identifier;

        #endregion

        #region IEntity

        public Guid Identifier
        {
            get { return _identifier; }
            set
            {
                if (_identifier != value) OnIDChanged();
                _identifier = value;
            }
        }

        public event EventHandler<EventArgs<IEntity>> IdentityChanged;

        #endregion

        #region Methods

        protected void OnIDChanged()
        {
            var caller = IdentityChanged;
            if (null == caller) return;
            var e = new EventArgs<IEntity>((IEntity)this);
            caller(this, e);
        }

        #endregion

    }
}
