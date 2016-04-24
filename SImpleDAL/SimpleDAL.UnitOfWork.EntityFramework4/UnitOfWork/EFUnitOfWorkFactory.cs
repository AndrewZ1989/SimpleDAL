using System;
using System.Data.Entity;
using SimpleDAL.UnitOfWork.EntityFramework.DataContext;
using SimpleDAL.Utility.Extensions;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
	public class EFUnitOfWorkFactory<TContext> : IUnitOfWorkFactory where TContext : DbContext, IDataContext
	{
		#region .ctor

		public EFUnitOfWorkFactory<TContext> SetDataContextProvider( Func<TContext> provider )
		{
			_dataContextProvider = provider;
			return this;
		}

		#endregion

		#region Fields

		private static Func<TContext> _dataContextProvider;

		#endregion

		#region IUnitOfWorkFactory

		public IUnitOfWork Create()
		{
			if ( _dataContextProvider == null ) throw new ArgumentNullException( "_dataContextProvider" );

			TContext context;
			context = _dataContextProvider();
			return new EFUnitOfWork<TContext>( new EFUnitOfWorkDataContext<TContext>( context ) );
		}
		#endregion
	}
}
