using System;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{

    public class LinqToSqlUnitOfWorkFactory<TDataContext> : IUnitOfWorkFactory where TDataContext : DataContext, IDataContext
	{
		#region fields
		private static Func<TDataContext> _dataContextProvider;
		#endregion

		#region methods

		public LinqToSqlUnitOfWorkFactory<TDataContext> SetDataContextProvider( Func<TDataContext> provider )
		{
			_dataContextProvider = provider;
			return this;
		}

		#endregion

		#region Implementation of IUnitOfWorkFactory

		public IUnitOfWork Create()
		{
			if ( _dataContextProvider == null ) throw new ArgumentNullException( "_dataContextProvider" );

			TDataContext context;
			context = _dataContextProvider();
			return new LinqToSqlUnitOfWork<TDataContext>( new LinqUnitOfWorkDataContext<TDataContext>( context ) );
		}
		#endregion
	}
}
