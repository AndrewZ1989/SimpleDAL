namespace SimpleDAL.UnitOfWork
{
    /// <summary>
    /// ��������� �����������, ������� ������ ���� ���������� � ������ �����������, ���������� � ������ � UoW
    /// </summary>
    /// <typeparam name="TBusinesEntity">���, �������������� �������� �� ������ ������-������</typeparam>
    /// /// <typeparam name="TEntityBuilder">���, �������������� ��������, ��������������� ������ ���������� ������-��������</typeparam>
    public interface IRepository<TBusinesEntity, TEntityBuilder> : IReadonlyRepository<TBusinesEntity>
	{
		/// <summary>
		/// ���������� ������, ������������� �� ������������ ��������
		/// </summary>
		TEntityBuilder CreateNew();

		/// <summary>
		/// ��������� ��������
		/// </summary>
		/// <param name="entity"></param>
		void Save( TBusinesEntity entity );

		/// <summary>
		/// ������� ��������
		/// </summary>
		/// <param name="entity"></param>
		void Delete( TBusinesEntity entity );

		/// <summary>
		/// ��������������� ��������
		/// </summary>
		/// <param name="entity"></param>
		void Refresh( TBusinesEntity entity );

		/// <summary>
		/// ������������ �������� � �����������
		/// </summary>
		TBusinesEntity Reload( TBusinesEntity entity, IInclude<TBusinesEntity> include );

		/// <summary>
		/// ������������ ��������
		/// </summary>
		TBusinesEntity Reload( TBusinesEntity entity );

	}
}