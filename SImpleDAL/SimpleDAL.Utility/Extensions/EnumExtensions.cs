using System;

namespace SimpleDAL.Utility.Extensions.EnumExtensions
{
    public static class EnumExtensions
	{
		/// <summary>
		/// Метод определяет, входит ли конкретное значение в перечисление, которое помечено атрибутом Flags
		/// </summary>
		/// <typeparam name="T">Любой enum</typeparam>
		/// <param name="flagEnum">исходное значение</param>
		/// <param name="enumValue">проверяемое на вхождение значение</param>
		/// <returns>true если входит, иначе false</returns>
		public static bool Contains<T>( this T flagEnum, T enumValue ) where T : struct
		{
			if ( !typeof( T ).IsEnum )
				throw new ArgumentException( "T is not a enum" );

			long lFlagEnum = Convert.ToInt64( flagEnum );
			long lEnumValue = Convert.ToInt64( enumValue );
			return ( lFlagEnum & lEnumValue ) == lEnumValue;
		}

		/// <summary>
		/// Метод определяет, входит ли конкретное значение в перечисление, которое помечено атрибутом Flags, исключая значение самого себя
		/// </summary>
		/// <typeparam name="T">Любой enum</typeparam>
		/// <param name="flagEnum">исходное значение</param>
		/// <param name="enumValue">проверяемое на вхождение значение</param>
		/// <returns>true если входит, иначе false</returns>
		public static bool ContainsExcludeSelf<T>( this T flagEnum, T enumValue ) where T : struct
		{
			if ( !typeof( T ).IsEnum )
				throw new ArgumentException( "T is not a enum" );

			long lFlagEnum = Convert.ToInt64( flagEnum );
			long lEnumValue = Convert.ToInt64( enumValue );
			return lFlagEnum != lEnumValue && ( lFlagEnum & lEnumValue ) == lEnumValue;
		}

	}
}
