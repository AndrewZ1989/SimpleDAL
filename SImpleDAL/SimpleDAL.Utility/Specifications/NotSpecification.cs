
namespace SimpleDAL.Utility.Specifications
{
	public class NotSpecification<T> : CompositeSpecification<T>
	{
		private readonly ISpecification<T> other;

		public NotSpecification( ISpecification<T> other )
		{
			this.other = other;
		}

		public override bool IsSatisfiedBy( T candidate )
		{
			return !other.IsSatisfiedBy( candidate );
		}
	}
}
