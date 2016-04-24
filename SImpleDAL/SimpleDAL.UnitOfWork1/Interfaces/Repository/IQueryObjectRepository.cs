namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// Интерфейс репозитория, поддерживающего запрос сущностей с использованием Query Object
    /// </summary>
    public interface IQueryObjectRepository<TBusinesEntity, TEntityBuilder, TQueryObject> 
        : IRepository<TBusinesEntity, TEntityBuilder>, IReadonlyQueryObjectRepository<TBusinesEntity, TQueryObject>
	{

	}
}
