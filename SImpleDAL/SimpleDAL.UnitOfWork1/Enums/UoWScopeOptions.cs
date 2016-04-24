using System;

namespace SimpleDAL.UnitOfWork
{
	/// <summary>
	/// ��������� ����� �������� �������������� �������
	/// </summary>
	[Flags]
	public enum UoWScopeOptions
	{
        /// <summary>
        /// ������������ ������������ �������������� �������. ���� ����� ��� - ������� �����.
        /// </summary>
		UseCompatible = 1,

        /// <summary>
        /// ������ ��������� ����� �������������� �������
        /// </summary>
		CreateNew = 2,

        /// <summary>
        /// ������������� ������������ ��������� ��� �������� �������������� �������
        /// </summary>
		AutoComplete = 4
	}
}