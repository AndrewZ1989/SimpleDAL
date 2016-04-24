namespace SimpleDAL.UnitOfWork
{
    public abstract class BaseUnitOfWorkService : IUnitOfWorkService
	{
		#region .ctor
		public BaseUnitOfWorkService( IUnitOfWorkFactory uowFactory )
		{
			_uowFactory = uowFactory;
		}
		#endregion

		#region Fields

		protected IUnitOfWorkFactory _uowFactory;

		#endregion

		#region IUnitOfWorkService

		public IReadonlyUnitOfWorkScope GetReadonlyScope()
		{
			return new UnitOfWorkScope( _uowFactory, UoWScopeOptions.UseCompatible );
		}

		public IUnitOfWorkScope GetReadWriteScope( bool autoComplete = false )
		{
			var options = UoWScopeOptions.CreateNew;
			if ( autoComplete ) options |= UoWScopeOptions.AutoComplete;

			return new UnitOfWorkScope( _uowFactory, options );
		}

		#endregion

	}
}
