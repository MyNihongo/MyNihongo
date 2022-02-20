using MyNihongo.SrcGen.Infrastructure.Database.Models;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class ColumnMappingRecordEx
{
	public static string ColumnPropertyOrThis(this ColumnMappingRecord @this) =>
		!string.IsNullOrEmpty(@this.ColumnProperty)
			? @this.ColumnProperty
			: "Self";
}