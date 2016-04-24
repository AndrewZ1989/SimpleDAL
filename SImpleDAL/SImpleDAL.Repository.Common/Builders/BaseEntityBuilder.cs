
namespace SimpleDAL.Repository.DAL.Builders
{

	public abstract class BaseEntityBuilder<TEntity> : IEntityBuilder<TEntity> where TEntity : class
	{
		#region .ctor
		public BaseEntityBuilder( TEntity entity )
		{
			_entity = entity;
		}
		#endregion

		#region Fields
		protected TEntity _entity;
		#endregion

		#region IBuilder<TEntity> Members

		TEntity IEntityBuilder<TEntity>.Done()
		{
			return _entity;
		}

		#endregion
	}
}
