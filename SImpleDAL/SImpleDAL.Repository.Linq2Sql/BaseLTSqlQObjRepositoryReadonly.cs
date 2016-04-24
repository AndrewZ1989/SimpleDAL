using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.UnitOfWork;
using SimpleDAL.UnitOfWork.Linq2Sql;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace SimpleDAL.Repository.DAL.Repositories
{

    public abstract class BaseLTSqlQObjRepositoryReadonly<TBusinesEntity, TStorageEntity, TDataContext, TQueryObject> : LinqToSqlRepositoryReadonly<TBusinesEntity, TStorageEntity, TDataContext>,
		IReadonlyQueryObjectRepository<TBusinesEntity, TQueryObject>

		where TStorageEntity : class, new()
		where TBusinesEntity : class, IEntity
		where TDataContext : DataContext
	{
		#region .ctor

		public BaseLTSqlQObjRepositoryReadonly( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}

		public BaseLTSqlQObjRepositoryReadonly( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TDataContext context )
			: base( mapperGenerator, context )
		{
		}

		#endregion

		#region Abstract

		protected abstract IQueryable<TStorageEntity> ExecuteQuery( TDataContext context, TQueryObject query );

		#endregion

		#region IQueryObjectRepository

		public IEnumerable<TBusinesEntity> Query( TQueryObject query )
		{
			var storeObjects = ExecuteQuery( DataContext, query );
			var mapper = GetMapper();
			return storeObjects.ToList().Select( s => mapper.MapStorageToBusines( s ) );
		}

		public IEnumerable<TBusinesEntity> Query( TQueryObject query, IInclude<TBusinesEntity> include )
		{
			var storeObjects = ExecuteQuery( DataContext, query );
			var mapper = GetMapper();

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
