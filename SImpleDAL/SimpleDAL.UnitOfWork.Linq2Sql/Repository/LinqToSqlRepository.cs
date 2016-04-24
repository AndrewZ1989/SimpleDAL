using System;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public abstract class LinqToSqlRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TDataContext> 
        : LinqToSqlRepositoryReadonly<TBusinesEntity, TStorageEntity, TDataContext>,
		IRepository<TBusinesEntity, TEntityBuilder>
		where TBusinesEntity : class
		where TStorageEntity : class
		where TDataContext : DataContext
	{
		#region fields

		private readonly DataLoadOptions _loadOptions = new DataLoadOptions();

		#endregion

		#region .ctor

		public LinqToSqlRepository( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}

		public LinqToSqlRepository( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TDataContext context )
			: base( mapperGenerator, context )
		{
			if ( context == null ) return;
		}

		public LinqToSqlRepository( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate ) : base( getMapperDelegate )
		{
		}

		public LinqToSqlRepository( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate, TDataContext context )
			: base( getMapperDelegate, context )
		{
		}


		#endregion

		#region IRepository

		public abstract TEntityBuilder CreateNew();

		public abstract void Save( TBusinesEntity entity );

		public abstract void Delete( TBusinesEntity entity );

		public abstract void Refresh( TBusinesEntity entity );

		public abstract TBusinesEntity Reload( TBusinesEntity entity, IInclude<TBusinesEntity> include );

		public abstract TBusinesEntity Reload( TBusinesEntity entity );

		#endregion

	}
}
