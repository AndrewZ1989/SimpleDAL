
namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Интерфейс маппера между сущностями бизнес-уровня и уровня хранилища данных
	/// </summary>
	public interface IRepositoryMapper<TBusinesEntity,TStorageEntity>
	{
		/// <summary>
		/// Отобразить сущность бизнес-уровня на сущность уровня хранилища
		/// </summary>
		void MapBusinesToStorage( TBusinesEntity bEntity, TStorageEntity sEntity );

		/// <summary>
		/// Отобразить сущность уровня хранилища на сущность бизнес-уровня
		/// </summary>
		void MapStorageToBusines( TStorageEntity sEntity, TBusinesEntity bEntity );
		void MapStorageToBusines( TStorageEntity sEntity, ref TBusinesEntity bEntity );

		/// <summary>
		/// Отобразить сущность уровня хранилища на сущность бизнес-уровня
		/// </summary>
		TBusinesEntity MapStorageToBusines( TStorageEntity sEntity );
	}
}
