using System;

namespace SimpleDAL.Utility.Specifications
{
	public interface ISpecification<T>
	{
		bool IsSatisfiedBy( T candidate );

		ISpecification<T> And( ISpecification<T> other );

		ISpecification<T> Or( ISpecification<T> other );

		ISpecification<T> Not();

		ISpecification<T> Condition( Func<ISpecification<T>, ISpecification<T>> func );
	}
}
