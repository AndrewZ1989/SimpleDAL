using System;
using SimpleDAL.Repository.DAL.Entities;
using SimpleDAL.Utility.Specifications;

namespace SimpleDAL.Repository.DAL.Specifications
{
	public class GetByIDSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : IEntity
	{
		public GetByIDSpecification( Guid id )
		{
			_id = id;
		}

		private Guid _id;

		public override bool IsSatisfiedBy( TEntity candidate )
		{
			return candidate.Identifier == _id;
		}
	}
}
