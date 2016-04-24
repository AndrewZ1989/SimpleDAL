using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.UnitOfWork;
using SimpleDAL.UnitOfWork.EntityFramework;
using SimpleDAL.UnitOfWork.EntityFramework.Entity;
using SimpleDAL.UnitOfWork.EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Objects;

namespace SimpleDAL.Repository.DAL.Repositories
{
    /// <summary>
    /// Абстрактный репозиторий Entity framework для бизнес-сущностей
    /// </summary>
    public abstract class BaseEFRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TContext> : EFRepository<TBusinesEntity, TStorageEntity, TEntityBuilder, TContext>
		where TBusinesEntity : class, IEntity
		where TStorageEntity : System.Data.Objects.DataClasses.EntityObject, IStoreEntity, new()
		where TContext : ObjectContext
	{
		#region .ctor
		public BaseEFRepository( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator )
			: base( mapperGenerator )
		{
		}
		public BaseEFRepository( Func<TContext, IRepositoryMapper<TBusinesEntity, TStorageEntity>> mapperGenerator, TContext context )
			: base( mapperGenerator, context )
		{
		}
		#endregion

		#region Abstract

		protected abstract void AdditionalActionsWhenDeleting( TBusinesEntity entity );

		protected abstract TEntityBuilder GetBuilder( TBusinesEntity entity );

		protected abstract int CompareStorageAndBusiness( TStorageEntity storeEntity, TBusinesEntity entity );

		protected abstract TBusinesEntity GetNewBusinessEntity();

		#endregion

		#region Override

		public override TEntityBuilder CreateNew()
		{
			var newEnt = GetNewBusinessEntity();
			newEnt.Identifier = Guid.NewGuid();
			return GetBuilder( newEnt );
		}

		public override void Save( TBusinesEntity entity )
		{
			if ( entity == null ) return;

			TStorageEntity targetEntity = null;
			foreach ( var storeEntity in Table )
			{
				if ( CompareStorageAndBusiness( storeEntity, entity ) == 0 )
				{
					targetEntity = storeEntity;
					break;
				}
			}

			if ( targetEntity == null )
			{
				targetEntity = new TStorageEntity();
				DataContext.AttachByIdValue<TStorageEntity>( targetEntity );
			}

			var mapper = _mapperGenerator( DataContext );
			mapper.MapBusinesToStorage( entity, targetEntity );
		}

		public override void Delete( TBusinesEntity entity )
		{
			using ( var enumerator = Table.GetEnumerator() )
			{
				TStorageEntity sEntity = GetEntityToDelete( entity, enumerator );
				if ( sEntity != null )
				{
					DataContext.DeleteObject( sEntity );
					AdditionalActionsWhenDeleting( entity );
				}
			}
		}

		public override void Refresh( TBusinesEntity entity )
		{
			var mapper = _mapperGenerator( DataContext );
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
			var mapper = _mapperGenerator( DataContext );
			var sEntity = Find( entity );
			if ( sEntity == null )
			{
				return mapper.MapStorageToBusines( sEntity );
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
