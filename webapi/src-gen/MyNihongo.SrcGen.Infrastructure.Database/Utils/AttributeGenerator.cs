using MyNihongo.SrcGen.Infrastructure.Database.Resources;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils;

internal static class AttributeGenerator
{
	public static string GenerateAttributes(this Compilation @this)
	{
		const string stringBuilderContext = $"{GeneratorConst.StoredProcedureContextAttributeName}Attribute",
			param = $"{GeneratorConst.ParamAttributeName}Attribute",
			paramTempTable = $"{GeneratorConst.ParamTempTableAttributeName}Attribute";

		var stringBuilder = new StringBuilder()
			.AppendNamespace(@this)
			.AppendLine("#nullable enable");

		// StringBuilderContext
		stringBuilder
			.AppendLine("[AttributeUsage(AttributeTargets.Class)]")
			.AppendFormat("internal sealed class {0} : Attribute", stringBuilderContext).AppendLine()
			.AppendLine("{")
			.AppendFormat("\tpublic {0}(string storedProcedureName, Type? returnType) {{ }}", stringBuilderContext).AppendLine()
			.AppendLine()
			.AppendFormat("\tpublic bool {0} {{ get; set; }}", GeneratorConst.IsNullableProp).AppendLine()
			.AppendLine("}")
			.AppendLine();

		// Param
		stringBuilder
			.AppendLine("[AttributeUsage(AttributeTargets.Property)]")
			.AppendFormat("internal sealed class {0} : Attribute", param).AppendLine()
			.AppendLine("{")
			.AppendFormat("\tpublic {0}(string parameterName) {{ }}", param).AppendLine()
			.AppendLine("}")
			.AppendLine();

		// Param - TempTable
		stringBuilder
			.AppendLine("[AttributeUsage(AttributeTargets.Property)]")
			.AppendFormat("internal sealed class {0} : Attribute", paramTempTable).AppendLine()
			.AppendLine("{")
			.AppendFormat("\tpublic {0}(string tempTableName) {{ }}", paramTempTable).AppendLine()
			.AppendLine()
			.AppendFormat("\tpublic string? {0} {{ get; init; }}", GeneratorConst.CollationProp).AppendLine()
			.AppendLine("}");

		return stringBuilder
			.AppendLine("#nullable disable")
			.ToString();
	}
}