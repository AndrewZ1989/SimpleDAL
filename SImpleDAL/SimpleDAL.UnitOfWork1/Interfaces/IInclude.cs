using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// Интерфейс единицы загрузки дополнительных данных. Используется для принудительной загрузки полей сущностей.
	/// </summary>
	public interface IInclude<TBusinesEntity>
	{
		void Include( TBusinesEntity entity );

		IInclude<TBusinesEntity> With<T>( Func<TBusinesEntity, T> expression );

		IInclude<TBusinesEntity> With( IInclude<TBusinesEntity> include );
	}
}
