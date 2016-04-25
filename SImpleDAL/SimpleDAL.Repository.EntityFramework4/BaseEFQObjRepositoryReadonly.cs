using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.UnitOfWork;
using SimpleDAL.UnitOfWork.EntityFramework;
using SimpleDAL.UnitOfWork.EntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SimpleDAL.Repository.DAL.Repositories
{
    public abstract class BaseEFQObjRepositoryReadonly<TBusinesEntity, TStorageEntity, TContext, TQueryObject> : EFRepositoryReadonly<TBusinesEntity, TStorageEntity, TContext>,
		IReadonlyQueryObjectRepository<TBusinesEntity, TQueryObject>

		where TBusinesEntity : class, IEntity
		where TStorageEntity : class, IStoreEntity, new()
		where TContext : DbContext
	{
		#region .ctor
		public BaseEFQObjRepositoryReadonly( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}

		public BaseEFQObjRepositoryReadonly( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TContext context )
			: base( mapperGenerator, context )
		{

		}
		#endregion

		#region Abstract

		protected abstract IQueryable<TStorageEntity> ExecuteQuery( TContext context, TQueryObject query );

		#endregion

		#region IQueryObjectRepository

		public IEnumerable<TBusinesEntity> Query( TQueryObject query )
		{
			var storeObjects = ExecuteQuery( DataContext, query );
			var mapper = _mapperGenerator( DataContext );
			return storeObjects.ToList().Select( s => mapper.MapStorageToBusines( s ) );
		}

		public IEnumerable<TBusinesEntity> Query( TQueryObject query, IInclude<TBusinesEntity> include )
		{
			var storeObjects = ExecuteQuery( DataContext, query );
			var mapper = _mapperGenerator( DataContext );

			return storeObjects.ToList().Select( s =>
			{
				var bEntity = mapper.MapStorageToBusines( s );
				include.Include( bEntity );
				return bEntity;
			} );
		}

		public long QueryCount( TQueryObject query )
		{
			return ExecuteQuery( DataContext, query ).Count();
		}

		#endregion

	}
}
