using SimpleDAL.Utility.Specifications;
using System.Collections.Generic;

namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// Интерфейс репозитория только для запросов, который должен быть реализован в каждом репозитории,
    ///  работающим в связке с UoW
    /// </summary>
    /// <typeparam name="TBusinesEntity">Тип, представляющий сущность на уровне бизнес-логики</typeparam>
    /// /// <typeparam name="TEntityBuilder">Тип, представляющий сущность, инкапсулирующую логику построения бизнес-сущности</typeparam>
    public interface IReadonlyRepository<TBusinesEntity>
	{
		IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification );

		/// <summary>
		/// запрос с прогрузкой сущности
		/// </summary>
		IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include );

		IEnumerable<TBusinesEntity> LazyQuery( ISpecification<TBusinesEntity> specification );

		/// <summary>
		/// запрос с прогрузкой сущности
		/// </summary>
		IEnumerable<TBusinesEntity> LazyQuery( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include );

		long QueryCount( ISpecification<TBusinesEntity> specification );
	}
}
