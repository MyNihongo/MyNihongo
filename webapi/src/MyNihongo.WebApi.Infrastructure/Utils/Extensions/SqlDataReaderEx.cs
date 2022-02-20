﻿// ReSharper disable once CheckNamespace
namespace Microsoft.Data.SqlClient;

internal static class SqlDataReaderEx
{
	public static T? GetNullableValue<T>(this SqlDataReader @this, int ordinal)
		where T : struct
	{
		var value = @this.GetValue(ordinal);

		return value == DBNull.Value
			? null
			: (T)value;
	}
}