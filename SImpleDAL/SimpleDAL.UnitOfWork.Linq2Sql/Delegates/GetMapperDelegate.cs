using System;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{

	public delegate IRepositoryMapper<TBusinesEntity, TStorageEntity> GetMapperDelegate<TDataContext, TBusinesEntity, TStorageEntity>( Func<TDataContext> dataContextAccessor )
		where TDataContext : DataContext
		where TBusinesEntity : class
		where TStorageEntity : class;


}
