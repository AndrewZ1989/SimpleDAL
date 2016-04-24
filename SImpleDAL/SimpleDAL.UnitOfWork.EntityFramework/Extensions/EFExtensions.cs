using SimpleDAL.UnitOfWork.EntityFramework.Entity;
using System.Data.Objects;

namespace SimpleDAL.UnitOfWork.EntityFramework.Extensions
{
    public static class EFExtensions
	{

		/// <summary>
		/// Связывает сущность Entity model с контекстом данных
		/// </summary>
		public static void AttachByIdValue<TEntity>( this ObjectContext context, TEntity rootEntity )
			where TEntity : class, IStoreEntity
		{
			context.AddObject( typeof( TEntity ).Name, rootEntity );

			if ( rootEntity.ID != 0 )
			{
				context.ObjectStateManager.GetObjectStateEntry( rootEntity ).SetModified();
			}
		}

	}
}
