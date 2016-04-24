using System;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Интерфейс транзакционной области кода только для чтения
	/// </summary>
	public interface IReadonlyUnitOfWorkScope : IDisposable
	{
		bool HasStarted { get; }

		IUnitOfWork RelUnitOfWork { get; }

		bool UseCompatibleEnabled { get; }
	}
}
