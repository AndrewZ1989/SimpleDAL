
namespace SimpleDAL.Repository.DAL.Builders
{
	public interface IEntityBuilder<TEntity> where TEntity : class
	{
		TEntity Done();
	}
}
