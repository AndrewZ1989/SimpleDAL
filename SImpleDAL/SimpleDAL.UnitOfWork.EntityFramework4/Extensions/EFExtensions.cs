using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using SimpleDAL.UnitOfWork.EntityFramework.Entity;
using System.Linq;

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
			context.AddObject( context.CreateObjectSet<TEntity>().EntitySet.Name, rootEntity );

			if ( rootEntity.ID != 0 )
			{
				context.ObjectStateManager.GetObjectStateEntry( rootEntity ).SetModified();
			}
		}

		public static IEnumerable<T> GetEntities<T>( this DbContext context, EntityState state )
		{
			return context.ChangeTracker.Entries().Where( e => e.State == state && e.GetType() == typeof( T ) ).Select( x => x.Entity ).Cast<T>();
		}

	}
}
