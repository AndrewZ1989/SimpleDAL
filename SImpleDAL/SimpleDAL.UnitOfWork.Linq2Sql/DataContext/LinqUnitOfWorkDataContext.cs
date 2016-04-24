using System;
using System.Data;
using System.Data.Common;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    internal class LinqUnitOfWorkDataContext<TDataContext> : ILinqSession<TDataContext> where TDataContext : DataContext, IDataContext
	{
		#region fields
		private bool _disposed;
		private TDataContext _context;
		#endregion

		#region .ctor
		public LinqUnitOfWorkDataContext( TDataContext context )
		{
			if ( context == null ) throw new ArgumentNullException( "context" );
			_context = context;
		}
		#endregion

		#region Implementation of ILinqSession

		public TDataContext Context
		{
			get { return _context; }
		}

		public IDbConnection Connection
		{
			get { return _context.Connection; }
		}

		public IDbTransaction Transaction
		{
			get { return _context.Transaction; }
			set { _context.Transaction = (DbTransaction)value; }
		}

		public void SubmitChanges()
		{
			( (IDataContext)_context ).SubmitChanges();
		}

		#endregion

		#region Implementation of IDisposable

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
