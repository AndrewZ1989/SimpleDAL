
namespace SimpleDAL.Utility.Specifications
{
	public class OrSpecification<T> : CompositeSpecification<T>
	{
        #region Fields

        private readonly ISpecification<T> left;
        private readonly ISpecification<T> right; 

        #endregion

        public OrSpecification( ISpecification<T> left, ISpecification<T> right )
		{
			this.left = left;
			this.right = right;
		}

		public override bool IsSatisfiedBy( T candidate )
		{
			return left.IsSatisfiedBy( candidate ) || right.IsSatisfiedBy( candidate );
		}
	}
}
