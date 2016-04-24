using System;
using System.Data;
using System.Data.Entity;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
	public interface IEFSession<TContext> : IDisposable where TContext : DbContext
	{
		TContext Context { get; }

		IDbConnection Connection { get; }

		void SubmitChanges();
	}
}
