namespace SimpleDAL.Utility.Specifications
{
    public class AndSpecification<T> : CompositeSpecification<T>
	{
		private readonly ISpecification<T> left;
		private readonly ISpecification<T> right;

		public AndSpecification( ISpecification<T> left, ISpecification<T> right )
		{
			this.left = left;
			this.right = right;
		}

		public override bool IsSatisfiedBy( T candidate )
		{
			return left.IsSatisfiedBy( candidate ) && right.IsSatisfiedBy( candidate );
		}
	}
}
