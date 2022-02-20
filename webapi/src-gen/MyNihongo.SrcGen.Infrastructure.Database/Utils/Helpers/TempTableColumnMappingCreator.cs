using System.Data;
using MyNihongo.SrcGen.Infrastructure.Database.Models;
using MyNihongo.SrcGen.Infrastructure.Database.Resources;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Helpers;

internal static class TempTableColumnMappingCreator
{
	public static IEnumerable<ColumnMappingRecord> CreateColumnMapping(this INamedTypeSymbol @this, IPropertySymbol prop, GeneratorExecutionContext ctx)
	{
		if (@this.IsSimpleType())
		{
			if (TryGetColumnType(@this.Name, out var columnType))
			{
				prop.TryGetAttrValue<string?>(GeneratorConst.ParamTempTableAttributeName, GeneratorConst.CollationProp, out var collation);
				yield return new ColumnMappingRecord(prop.Name, string.Empty, $"[{@this.Name.ToLowerInvariant()}]", columnType, collation);
			}
			else
			{
				ctx.ReportError($"Unhandled column type: {@this.Name}", prop);
			}
		}
		else
		{
			ctx.ReportError($"Unhandled class types: {@this.Name}", @this);
		}
	}

	private static bool TryGetColumnType(in string typeName, out ColumnMappingRecord.ColType columnType)
	{
		columnType = typeName switch
		{
			"Char" => new ColumnMappingRecord.ColType(SqlDbType.NChar, 1),
			_ => default!
		};

		return columnType != null!;
	}
}