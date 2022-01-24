using Microsoft.CodeAnalysis.CSharp.Syntax;
using MyNihongo.SrcGen.Migrations.Resources;
using MyNihongo.SrcGen.Migrations.Utils.Extensions;

namespace MyNihongo.SrcGen.Migrations.Utils;

internal static class SourceGenerator
{
	public static string GenerateSource(in this GeneratorExecutionContext @this, IReadOnlyList<TypeDeclarationSyntax> types)
	{
		const string thisVar = "@this",
			rootVar = "root",
			itemsVar = "items";

		var stringBuilder = new StringBuilder()
			.AppendNamespace(@this.Compilation)
			.AppendLine("internal static class IInsertExpressionRootEx")
			.AppendLine("{");

		foreach (var typeSyntax in types)
		{
			var typeSymbol = typeSyntax.GetTypeSymbol(@this.Compilation);

			if (!typeSymbol.TryGetAttributeValue(GeneratorConst.TableAttributeName, out var tableName))
			{
				@this.ReportError($"Cannot find an attribute `{GeneratorConst.TableAttributeName}` for `{typeSymbol}`", typeSymbol);
				continue;
			}

			stringBuilder
				.AppendFormat("\tpublic static void Rows(this FluentMigrator.Builders.Insert.IInsertExpressionRoot {0}, IReadOnlyList<{1}> {2})", thisVar, typeSymbol, itemsVar).AppendLine()
				.AppendLine("\t{")
				.AppendFormat("\t\tvar {0} = {1}.IntoTable(\"{2}\");", rootVar, thisVar, tableName).AppendLine()
				.AppendFormat("\t\tfor (var i = 0; i < {0}.Count; i++)", itemsVar).AppendLine()
				.AppendFormat("\t\t\t{0}.Row(new Dictionary<string, object>", rootVar).AppendLine()
				.AppendLine("\t\t\t{");

			foreach (var prop in typeSymbol.GetProperties())
			{
				if (!prop.TryGetAttributeValue(GeneratorConst.ColumnAttributeName, out var colName))
				{
					@this.ReportError($"Cannot find an attribute `{GeneratorConst.ColumnAttributeName}` for a property `{prop.Name}`", prop);
					continue;
				}

				var cast = prop.Type.TryGetEnumUnderlyingType(out var enumTypeSymbol)
					? $"({enumTypeSymbol})"
					: string.Empty;

				stringBuilder
					.AppendFormat("\t\t\t\t{{ \"{0}\", {1}{2}[i].{3} }},", colName, cast, itemsVar, prop.Name)
					.AppendLine();
			}

			stringBuilder
				.AppendLine("\t\t\t});")
				.AppendLine("\t}");
		}

		return stringBuilder
			.AppendLine("}")
			.ToString();
	}

	private static bool TryGetAttributeValue(this ISymbol typeSymbol, string attrName, out string value)
	{
		value = string.Empty;

		var attrs = typeSymbol.GetAttributes();
		for (var i = 0; i < attrs.Length; i++)
		{
			if (attrs[i].GetName() != attrName)
				continue;

			for (var j = 0; j < attrs[i].ConstructorArguments.Length; j++)
			{
				if (attrs[i].ConstructorArguments[j].Value is not string strValue)
					continue;

				value = strValue;
				goto Return;

			}
		}

		Return:
		return !string.IsNullOrEmpty(value);
	}
}