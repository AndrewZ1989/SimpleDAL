using System;
using System.Data.Linq;

namespace SimpleDAL.UnitOfWork.Linq2Sql
{
    public abstract class LinqToSqlMapper<TBusinesEntity, TStorageEntity, TDataContext> 
        : IRepositoryMapper<TBusinesEntity, TStorageEntity> where TDataContext : DataContext
	{
		#region .ctor
		public LinqToSqlMapper( TDataContext context )
		{
			_dataContext = context;
		}

		public LinqToSqlMapper( Func<TDataContext> contextAccessor )
		{
			_contextAccessor = contextAccessor;
		}

		#endregion

		#region Fields

		private TDataContext _dataContext;

		private Func<TDataContext> _contextAccessor;

		#endregion

		#region Properties

		protected TDataContext DataContext
		{
			get
			{
				if ( _dataContext != null ) return _dataContext;

				if ( _contextAccessor != null ) return _contextAccessor.Invoke();

				throw new Exception("Can not create a data context for the mapping.");
			}
		}

		#endregion

		#region Public methods

		public abstract void MapBusinesToStorage( TBusinesEntity bEntity, TStorageEntity sEntity );

		public abstract void MapStorageToBusines( TStorageEntity sEntity, TBusinesEntity bEntity );

		public void MapStorageToBusines( TStorageEntity sEntity, ref TBusinesEntity bEntity )
		{
			MapStorageToBusines( sEntity, bEntity );
		}

		public abstract TBusinesEntity MapStorageToBusines( TStorageEntity sEntity );

		#endregion
	}
}
