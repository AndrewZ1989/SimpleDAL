using System;
using System.Data;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public class LinqToSqlUnitOfWork<TDataContext> : IUnitOfWork where TDataContext : DataContext
	{
		#region fields
		private bool _disposed;
		private ILinqSession<TDataContext> _linqContext;
		private LinqToSqlTransaction _transaction;
		#endregion

		#region ctor

		public LinqToSqlUnitOfWork( ILinqSession<TDataContext> context )
		{
			if ( context == null ) throw new ArgumentNullException( "context" );

			_linqContext = context;
		}
		#endregion

		#region properties

		public TDataContext Context
		{
			get { return _linqContext.Context; }
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
			if ( !disposing ) return;
			if ( _disposed ) return;

			if ( _transaction != null )
			{
				_transaction.Dispose();
				_transaction = null;
			}
			if ( _linqContext != null )
			{
				_linqContext.Dispose();
				_linqContext = null;
			}
			_disposed = true;
		}
		#endregion

		#region Implementation of IUnitOfWork

		public bool IsInTransaction
		{
			get { return _transaction != null; }
		}

		public ITransaction BeginTransaction()
		{
			return BeginTransaction( IsolationLevel.ReadCommitted );
		}

		public ITransaction BeginTransaction( IsolationLevel isolationLevel )
		{
			if ( _transaction != null )
			{
				throw new InvalidOperationException( "Cannot start transaction." );
			}

			if ( !_linqContext.Context.DatabaseExists() )
			{
				throw new InvalidOperationException( string.Format("Database {0} is unavailable.", _linqContext.Context.Connection.Database ) );
			}

			if ( _linqContext.Connection.State != ConnectionState.Open )
				_linqContext.Connection.Open();

			IDbTransaction transaction = _linqContext.Connection.BeginTransaction( isolationLevel );
			_linqContext.Transaction = transaction;
			_transaction = new LinqToSqlTransaction( transaction );
			_transaction.TransactionCommitted += TransactionCommitted;
			_transaction.TransactionRolledback += TransactionRolledback;
			return _transaction;
		}

		public void Flush()
		{
			_linqContext.SubmitChanges();
		}

		public void TransactionalFlush()
		{
			TransactionalFlush( IsolationLevel.ReadCommitted );
		}

		public void TransactionalFlush( IsolationLevel isolationLevel )
		{
			if ( !IsInTransaction )
				BeginTransaction( isolationLevel );
			try
			{
				_linqContext.SubmitChanges();
				_transaction.Commit();
			}
			catch
			{
				_transaction.Rollback();
				throw;
			}
		}
		#endregion

		#region methods

		private void TransactionRolledback( object sender, EventArgs e )
		{
			if ( !sender.Equals( _transaction ) )
			{
				throw new InvalidOperationException();
			}
			ReleaseCurrentTransaction();
		}

		private void TransactionCommitted( object sender, EventArgs e )
		{
			if ( !sender.Equals( _transaction ) )
			{
				throw new InvalidOperationException();
			}
			ReleaseCurrentTransaction();
		}

		private void ReleaseCurrentTransaction()
		{
			if ( _transaction != null )
			{
				_transaction.TransactionCommitted -= TransactionCommitted;
				_transaction.TransactionRolledback -= TransactionRolledback;
				_transaction.Dispose();
			}
			_transaction = null;

			if ( _linqContext.Connection.State == ConnectionState.Open )
				_linqContext.Connection.Close();
		}

		#endregion
	}
}
