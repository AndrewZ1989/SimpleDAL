using System;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Интерфейс транзакционной области кода
	/// </summary>
	public interface IUnitOfWorkScope : IReadonlyUnitOfWorkScope
	{
		/// <summary>
		/// Подтвердить внесенные изменения
		/// </summary>
		void Commit();
	}
}
