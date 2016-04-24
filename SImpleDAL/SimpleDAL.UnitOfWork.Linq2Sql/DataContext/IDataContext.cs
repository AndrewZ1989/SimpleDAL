namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public interface IDataContext
	{
		void SubmitChanges();
	}
}
