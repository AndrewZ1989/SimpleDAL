using SimpleDAL.UnitOfWork.EntityFramework.DataContext;
using System;
using System.Data;
using System.Data.Objects;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
    internal class EFUnitOfWorkDataContext<TContext> : IEFSession<TContext> where TContext : ObjectContext, IDataContext
	{
		#region .ctor

		public EFUnitOfWorkDataContext( TContext context )
		{
			if ( context == null ) throw new ArgumentNullException( "context" );
			_context = context;
		}

		#endregion

		#region Fields

		private bool _disposed;

		private TContext _context;

		#endregion

		#region IEntityFrameworkSession

		public TContext Context
		{
			get { return _context; }
		}

		public IDbConnection Connection
		{
			get { return _context.Connection; }
		}

		public void SubmitChanges()
		{
			( (IDataContext)_context ).SaveChanges();
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		private void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( !_disposed )
				{
					_context.Dispose();
					_disposed = true;
				}
			}
		}

		#endregion
	}
}
