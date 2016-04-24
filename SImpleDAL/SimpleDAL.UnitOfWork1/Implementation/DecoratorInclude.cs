using System;

namespace SimpleDAL.UnitOfWork
{
    abstract public class DecoratorInclude<TEntity> : IInclude<TEntity>
	{
		#region .ctor

		public DecoratorInclude()
		{
			_include = new BaseInclude<TEntity>();
			Configurate();
		}

        #endregion

        #region Fields

        protected IInclude<TEntity> _include;

        #endregion

        #region IInclude

        public void Include( TEntity entity )
		{
			_include.Include( entity );
		}

		public IInclude<TEntity> With<T>( Func<TEntity, T> expression )
		{
			_include.With( expression );
			return this;
		}

		public IInclude<TEntity> With( IInclude<TEntity> include )
		{
			_include.With( include );
			return this;
		}

		#endregion

		#region Abstract

		protected abstract void Configurate();

		#endregion
	}
}
