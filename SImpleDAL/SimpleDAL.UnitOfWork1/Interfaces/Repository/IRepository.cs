namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// Интерфейс репозитория, который должен быть реализован в каждом репозитории, работающим в связке с UoW
    /// </summary>
    /// <typeparam name="TBusinesEntity">Тип, представляющий сущность на уровне бизнес-логики</typeparam>
    /// /// <typeparam name="TEntityBuilder">Тип, представляющий сущность, инкапсулирующую логику построения бизнес-сущности</typeparam>
    public interface IRepository<TBusinesEntity, TEntityBuilder> : IReadonlyRepository<TBusinesEntity>
	{
		/// <summary>
		/// Возвращает объект, ответственный за формирование сущности
		/// </summary>
		TEntityBuilder CreateNew();

		/// <summary>
		/// Сохранить сущность
		/// </summary>
		/// <param name="entity"></param>
		void Save( TBusinesEntity entity );

		/// <summary>
		/// Удалить сущность
		/// </summary>
		/// <param name="entity"></param>
		void Delete( TBusinesEntity entity );

		/// <summary>
		/// Актуализировать сущность
		/// </summary>
		/// <param name="entity"></param>
		void Refresh( TBusinesEntity entity );

		/// <summary>
		/// перезагрузка сущности с параметрами
		/// </summary>
		TBusinesEntity Reload( TBusinesEntity entity, IInclude<TBusinesEntity> include );

		/// <summary>
		/// перезагрузка сущности
		/// </summary>
		TBusinesEntity Reload( TBusinesEntity entity );

	}
}