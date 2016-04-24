using System;
using System.Data;
using System.Data.Objects;

namespace SimpleDAL.UnitOfWork.EntityFramework
{
    public interface IEFSession<TContext> : IDisposable where TContext : ObjectContext
	{
		TContext Context { get; }

		IDbConnection Connection { get; }

		void SubmitChanges();
	}
}
