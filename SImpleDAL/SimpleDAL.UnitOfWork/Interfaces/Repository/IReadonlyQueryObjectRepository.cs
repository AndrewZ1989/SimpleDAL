using System.Collections.Generic;

namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// Интерфейс репозитория только для чтения, поддерживающего запрос сущностей с использованием Query Object
    /// </summary>
    public interface IReadonlyQueryObjectRepository<TBusinesEntity, TQueryObject> : IReadonlyRepository<TBusinesEntity>
	{
		IEnumerable<TBusinesEntity> Query( TQueryObject query );

		IEnumerable<TBusinesEntity> Query( TQueryObject query, IInclude<TBusinesEntity> include );

		long QueryCount( TQueryObject query );
	}
}
