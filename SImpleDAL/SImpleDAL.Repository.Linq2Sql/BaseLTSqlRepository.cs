using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.UnitOfWork;
using SimpleDAL.UnitOfWork.Linq2Sql;
using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace SimpleDAL.Repository.DAL.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий LinqToSql для бизнес-сущностей
    /// </summary>
    public abstract class BaseLTSqlRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TDataContext> : LinqToSqlRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TDataContext>
		where TStorageEntity : class, new()
		where TBusinesEntity : class, IEntity
		where TDataContext : DataContext
	{
		#region .ctor
		public BaseLTSqlRepository( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}
		public BaseLTSqlRepository( Func<TDataContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TDataContext context )
			: base( mapperGenerator, context )
		{
		}

		public BaseLTSqlRepository( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate ) : base( getMapperDelegate )
		{
		}

		public BaseLTSqlRepository( GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity> getMapperDelegate, TDataContext context )
			: base( getMapperDelegate, context )
		{
		}


		#endregion

		#region Abstract

		protected abstract void AdditionalActionsWhenDeleting( TBusinesEntity entity );
		protected abstract TEntityBuilder GetBuilder( TBusinesEntity entity );
		protected abstract TBusinesEntity GetNewBusinessEntity();
		protected abstract int CompareStorageAndBusiness( TStorageEntity storeEntity, TBusinesEntity entity );

		#endregion

		#region LinqToSqlRepository impl

		public override TEntityBuilder CreateNew()
		{
			var newEnt = GetNewBusinessEntity();
			newEnt.Identifier = Guid.NewGuid();
			return GetBuilder( newEnt );
		}

		public override void Save( TBusinesEntity entity )
		{
			if ( entity == null ) return;

			var mapper = GetMapper();
			foreach ( var storeEntity in Table )
			{
				if ( CompareStorageAndBusiness( storeEntity, entity ) == 0 )
				{
					mapper.MapBusinesToStorage( entity, storeEntity );
					return;
				}
			}

			TStorageEntity newEnt = new TStorageEntity();
			mapper.MapBusinesToStorage( entity, newEnt );
			Table.InsertOnSubmit( newEnt );
		}

		public override void Delete( TBusinesEntity entity )
		{
			using ( var enumerator = Table.GetEnumerator() )
			{
				TStorageEntity sEntity = GetEntityToDelete( entity, enumerator );
				if ( sEntity != null )
				{
					Table.DeleteOnSubmit( sEntity );
					AdditionalActionsWhenDeleting( entity );
				}
			}
		}

		public override void Refresh( TBusinesEntity entity )
		{
			var mapper = GetMapper();
			foreach ( var storeEntity in Table )
			{
				if ( CompareStorageAndBusiness( storeEntity, entity ) == 0 )
				{
					mapper.MapStorageToBusines( storeEntity, entity );
					return;
				}
			}
		}

		public override TBusinesEntity Reload( TBusinesEntity entity )
		{
			var mapper = GetMapper();
			var sEntity = Find( entity );
			if ( sEntity == null )
			{
				return ConfigureBeforeReturn( mapper.MapStorageToBusines( sEntity ) );
			}
			else
			{
				return entity;
			}
		}

		public override TBusinesEntity Reload( TBusinesEntity entity, IInclude<TBusinesEntity> include )
		{
			var reloadEntity = Reload( entity );
			include.Include( reloadEntity );
			return reloadEntity;
		}

		protected abstract TStorageEntity Find( TBusinesEntity bEntity );

		#endregion

		#region Private methods

		private TStorageEntity GetEntityToDelete( TBusinesEntity entity, IEnumerator<TStorageEntity> enumerator )
		{
			while ( enumerator.MoveNext() )
			{
				if ( CompareStorageAndBusiness( enumerator.Current, entity ) == 0 )
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		#endregion
	}
}
