using SimpleDAL.Utility;
using System;

namespace SimpleDAL.Repository.DAL.Entities
{
    /// <summary>
    /// Общий интерфейс бизнес-сущности
    /// </summary>
    public interface IEntity
	{
		/// <summary>
		/// Уникальный идентификатор сущности
		/// </summary>
		Guid Identifier { get; set; }

        /// <summary>
        /// Событие изменения уникального идентификатора
        /// </summary>
		event EventHandler<EventArgs<IEntity>> IdentityChanged;
	}
}
