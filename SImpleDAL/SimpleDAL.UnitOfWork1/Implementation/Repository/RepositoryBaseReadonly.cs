using SimpleDAL.Utility.Specifications;
using System.Collections.Generic;

namespace SimpleDAL.UnitOfWork
{
    public abstract class RepositoryBaseReadonly<TBusinesEntity, TStorageEntity> : IReadonlyRepository<TBusinesEntity>
	{
		#region .ctor

		public RepositoryBaseReadonly()
		{
		}

		#endregion

		#region Methods

		protected virtual TUnitOfWork GetCurrentUnitOfWork<TUnitOfWork>() where TUnitOfWork : IUnitOfWork
		{
			return ( (TUnitOfWork)UnitOfWork.Current );
		}

		#endregion

		#region Implementation of IReadonlyRepository<TBusinesEntity>

		public abstract IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification );

		public abstract IEnumerable<TBusinesEntity> Query( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include );

		public abstract IEnumerable<TBusinesEntity> LazyQuery( ISpecification<TBusinesEntity> specification );

		public abstract IEnumerable<TBusinesEntity> LazyQuery( ISpecification<TBusinesEntity> specification, IInclude<TBusinesEntity> include );

		public abstract long QueryCount( ISpecification<TBusinesEntity> specification );

		#endregion
	}
}