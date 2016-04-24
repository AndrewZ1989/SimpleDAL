using System;
using System.Data.Objects;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
    public abstract class EFRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TContext> : EFRepositoryReadonly<TBusinesEntity, TStorageEntity, TContext>,
		IRepository<TBusinesEntity, TEntityBuilder>
		where TBusinesEntity : class
		where TStorageEntity : class
		where TContext : ObjectContext
	{
		#region .ctor

		public EFRepository( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}

		public EFRepository( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TContext context )
			: base( mapperGenerator, context )
		{
		}

		#endregion

		#region Overrides of RepositoryBase

		public abstract TEntityBuilder CreateNew();

		public abstract void Save( TBusinesEntity entity );

		public abstract void Delete( TBusinesEntity entity );

		public abstract void Refresh( TBusinesEntity entity );

		public abstract TBusinesEntity Reload( TBusinesEntity entity, IInclude<TBusinesEntity> include );

		public abstract TBusinesEntity Reload( TBusinesEntity entity );

		#endregion

	}
}
