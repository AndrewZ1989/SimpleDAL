using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace SimpleDAL.UnitOfWork.EntityFramework.Mappers
{
    public abstract class EntityMapper<TBusinesEntity, TStorageEntity, TContext> : IRepositoryMapper<TBusinesEntity, TStorageEntity>
		where TContext : ObjectContext
		where TStorageEntity : EntityObject
	{
		#region .ctor
		public EntityMapper( TContext context )
		{
			_dataContext = context;
		}
		#endregion

		#region Fields

		protected TContext _dataContext;

		#endregion

		#region Abstract and virtual

		protected abstract TBusinesEntity CreateBusinesEntity( TStorageEntity sEntity );

		protected virtual bool IsNotNewEntity( TStorageEntity sEntity )
		{
			return sEntity.EntityState != System.Data.EntityState.Added && sEntity.EntityState != System.Data.EntityState.Detached;
		}

		#endregion

		#region IRepositoryMapper

		public abstract void MapBusinesToStorage( TBusinesEntity bEntity, TStorageEntity sEntity );

		public abstract void MapStorageToBusines( TStorageEntity sEntity, TBusinesEntity bEntity );

		public abstract void MapStorageToBusines( TStorageEntity sEntity, ref TBusinesEntity bEntity );

		public TBusinesEntity MapStorageToBusines( TStorageEntity sEntity )
		{
			TBusinesEntity newBusinesEntity = CreateBusinesEntity( sEntity );
			MapStorageToBusines( sEntity, newBusinesEntity );
			return newBusinesEntity;
		}

		#endregion

	}
}
