namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Интерфейс фабрики UoW
	/// </summary>
	public interface IUnitOfWorkFactory
	{
		IUnitOfWork Create();
	}
}