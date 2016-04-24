using System;
using System.Data;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public class LinqToSqlTransaction : ITransaction
	{
		#region fields

		private bool _disposed;
		private readonly IDbTransaction _internalTransaction;

		#endregion

		#region ctor

		public LinqToSqlTransaction( IDbTransaction transaction )
		{
			if ( transaction == null ) throw new ArgumentNullException( "transaction" );

			_internalTransaction = transaction;
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
					_internalTransaction.Dispose();
					_disposed = true;
				}
			}
		}
		#endregion

		#region Implementation of ITransaction

		public event EventHandler TransactionCommitted;

		public event EventHandler TransactionRolledback;

		public void Commit()
		{
			if ( _disposed ) throw new ObjectDisposedException( "LinqToSqlTransaction", "Cannot commit a disposed transaction." );

			_internalTransaction.Commit();
			if ( TransactionCommitted != null )
				TransactionCommitted( this, EventArgs.Empty );
		}

		public void Rollback()
		{
			if ( _disposed ) throw new ObjectDisposedException( "LinqToSqlTransaction", "Cannot rollback a disposed transaction." );

			_internalTransaction.Rollback();
			if ( TransactionRolledback != null )
				TransactionRolledback( this, EventArgs.Empty );
		}
		#endregion
	}
}
