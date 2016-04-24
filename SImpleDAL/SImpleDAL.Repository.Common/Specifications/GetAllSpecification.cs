using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.Utility.Specifications;

namespace SimpleDAL.Repository.DAL.Specifications
{
	public class GetAllSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : IEntity
	{
		public GetAllSpecification() { }

		public override bool IsSatisfiedBy( TEntity candidate )
		{
			return true;
		}
	}
}
