using System;
using System.Data;
using System.Data.Objects;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
    internal class EFUnitOfWork<TContext> : IUnitOfWork where TContext : ObjectContext
	{
		#region .ctor

		public EFUnitOfWork( IEFSession<TContext> context )
		{
			if ( context == null ) throw new ArgumentNullException( "context" );
			_entityFrameworkContext = context;
		}

		#endregion

		#region Fields

		private bool _disposed;

		private IEFSession<TContext> _entityFrameworkContext;

		private EFTransaction _transaction;

		#endregion

		#region Properties

		public TContext Context
		{
			get { return _entityFrameworkContext.Context; }
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
			if ( !disposing ) return;
			if ( _disposed ) return;

			if ( _transaction != null )
			{
				_transaction.Dispose();
				_transaction = null;
			}
			if ( _entityFrameworkContext != null )
			{
				_entityFrameworkContext.Dispose();
				_entityFrameworkContext = null;
			}
			_disposed = true;
		}
		#endregion

		#region IUnitOfWork

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

			if ( _entityFrameworkContext.Connection.State != ConnectionState.Open )
				_entityFrameworkContext.Connection.Open();

			IDbTransaction transaction = _entityFrameworkContext.Connection.BeginTransaction( isolationLevel );
			_transaction = new EFTransaction( transaction );
			_transaction.TransactionCommitted += TransactionCommitted;
			_transaction.TransactionRolledback += TransactionRolledback;
			return _transaction;
		}

		public void Flush()
		{
			_entityFrameworkContext.SubmitChanges();
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
				_entityFrameworkContext.SubmitChanges();
				_transaction.Commit();
			}
			catch
			{
				_transaction.Rollback();
				throw;
			}
		}
		#endregion

		#region Private methods

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

			if ( _entityFrameworkContext.Connection.State == ConnectionState.Open )
				_entityFrameworkContext.Connection.Close();
		}

		#endregion
	}
}
