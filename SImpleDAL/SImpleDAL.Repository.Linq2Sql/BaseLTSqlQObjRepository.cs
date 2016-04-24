using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.UnitOfWork;
using SimpleDAL.UnitOfWork.Linq2Sql;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace SimpleDAL.Repository.DAL.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий LinqToSql для бизнес-сущностей, поддерживающий запрос сущностей через Query object
    /// </summary>
    public abstract class BaseLTSqlQObjRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TDataContext, TQueryObject> : BaseLTSqlRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TDataContext>,
		IQueryObjectRepository<TBusinesEntity, TEntityBuilder, TQueryObject>

		where TStorageEntity : class, new()
		where TBusinesEntity : class, IEntity
		where TDataContext : DataContext
	{
		#region .ctor

		public BaseLTSqlQObjRepository( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}

		public BaseLTSqlQObjRepository( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TDataContext context )
			: base( mapperGenerator, context )
		{
		}

		public BaseLTSqlQObjRepository( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate ) : base( getMapperDelegate )
		{
		}

		public BaseLTSqlQObjRepository( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate, TDataContext context )
			: base( getMapperDelegate, context )
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
			return storeObjects.ToList().Select( s => ConfigureBeforeReturn( mapper.MapStorageToBusines( s ) ) );
		}

		public IEnumerable<TBusinesEntity> Query( TQueryObject query, IInclude<TBusinesEntity> include )
		{
			var storeObjects = ExecuteQuery( DataContext, query );
			var mapper = GetMapper();

			return storeObjects.ToList().Select( s =>
			{
				var bEntity = mapper.MapStorageToBusines( s );
				include.Include( bEntity );
				return ConfigureBeforeReturn( bEntity );
			} );
		}

		public long QueryCount( TQueryObject query )
		{
			var collection = ExecuteQuery( DataContext, query );
			return collection.Count();
		}

		#endregion
	}
}
