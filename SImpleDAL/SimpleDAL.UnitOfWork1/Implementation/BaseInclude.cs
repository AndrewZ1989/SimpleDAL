using System;
using System.Collections.Generic;

namespace SimpleDAL.UnitOfWork
{
    public class BaseInclude<TBusinesEntity> : IInclude<TBusinesEntity>
	{
        #region .ctor

        public BaseInclude()
        {
            _expressions = new List<Func<TBusinesEntity, object>>();
            _Includes = new List<IInclude<TBusinesEntity>>();
        }

        #endregion

        #region Fields

        protected List<Func<TBusinesEntity, object>> _expressions;
        protected List<IInclude<TBusinesEntity>> _Includes;

        #endregion

        #region IInclude

        public void Include(TBusinesEntity entity)
        {
            _expressions.ForEach(e => e(entity));
            _Includes.ForEach(e => e.Include(entity));
        }

        public IInclude<TBusinesEntity> With<T>(Func<TBusinesEntity, T> expression)
        {
            _expressions.Add(e => expression(e));
            return this;
        }

        public IInclude<TBusinesEntity> With(IInclude<TBusinesEntity> include)
        {
            _Includes.Add(include);
            return this;
        } 

        #endregion
    }
}
