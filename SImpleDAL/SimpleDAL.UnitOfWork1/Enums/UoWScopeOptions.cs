using System;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Различные опции создания транзакционной области
	/// </summary>
	[Flags]
	public enum UoWScopeOptions
	{
        /// <summary>
        /// Использовать существующую транзакционную область. Если такой нет - создать новую.
        /// </summary>
		UseCompatible = 1,

        /// <summary>
        /// Свегда создавать новую транзакционную область
        /// </summary>
		CreateNew = 2,

        /// <summary>
        /// Автоматически подтверждать изменения при закрытии транзакционной области
        /// </summary>
		AutoComplete = 4
	}
}