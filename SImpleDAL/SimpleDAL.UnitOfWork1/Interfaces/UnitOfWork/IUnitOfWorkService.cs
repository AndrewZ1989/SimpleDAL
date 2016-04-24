using System.Data;

namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// Сервис, предоставляющий возможность создания области бизнес-транзакции
    /// </summary>
	public interface IUnitOfWorkService
    {
        /// <summary>
        /// Получить транзакционную область, в которой возможно только чтение
        /// </summary>
        /// <returns></returns>
        IReadonlyUnitOfWorkScope GetReadonlyScope();

        /// <summary>
        /// Получить транзакционную область с возможностью чтения и записи
        /// </summary>
        /// <param name="autoComplete">Автоматически подтверждать изменения при выходе из области</param>
        /// <returns></returns>
        IUnitOfWorkScope GetReadWriteScope(bool autoComplete = false);
    }
}
