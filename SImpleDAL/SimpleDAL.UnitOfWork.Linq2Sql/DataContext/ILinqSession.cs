using System;
using System.Data;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public interface ILinqSession<TDataContext> : IDisposable where TDataContext : DataContext
	{
		TDataContext Context { get; }

		IDbConnection Connection { get; }

		IDbTransaction Transaction { get; set; }

		void SubmitChanges();
	}

}
