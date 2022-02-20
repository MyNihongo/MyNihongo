using System.Collections.Immutable;
using MyNihongo.SrcGen.Infrastructure.Database.Enums;
using MyNihongo.SrcGen.Infrastructure.Database.Models;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Helpers;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils;

internal static class SourceGenerator
{
	private const string ConnectionVar = "connection",
		CommandVar = "command",
		ReaderVar = "reader",
		ConfigurationField = "_configuration";

	public static SourceGenerationResult GenerateSource(this Compilation @this, in GeneratorExecutionContext ctx, string groupKey, IReadOnlyList<MethodDeclarationRecord> methodDeclarations)
	{
		var indent = 1;

		string className = $"{groupKey}DatabaseService",
			interfaceName = $"I{className}";

		var stringBuilder = new StringBuilder()
			.AppendNamespace(@this, groupKey);

		// Interface
		stringBuilder
			.AppendFormat("internal interface {0}", interfaceName).AppendLine()
			.AppendLine("{");

		for (var i = 0; i < methodDeclarations.Count; i++)
		{
			if (i != 0)
				stringBuilder.AppendLine();

			GenerateInterfaceMethod(stringBuilder, indent, methodDeclarations[i], ctx);
		}

		stringBuilder
			.AppendLine("}")
			.AppendLine();

		// Implementation
		stringBuilder
			.AppendFormat("internal sealed class {0} : {1}", className, interfaceName).AppendLine()
			.AppendLine("{")
			.Indent(indent).AppendFormat("private readonly IConfiguration {0};", ConfigurationField).AppendLine()
			.AppendLine()
			.Indent(indent).AppendFormat("public {0}(IConfiguration configuration)", className).AppendLine()
			.Indent(indent++).AppendLine("{")
			.Indent(indent).AppendFormat("{0} = configuration;", ConfigurationField).AppendLine()
			.Indent(--indent).AppendLine("}");

		for (var i = 0; i < methodDeclarations.Count; i++)
		{
			stringBuilder.AppendLine();
			GenerateClassMethod(stringBuilder, indent, methodDeclarations[i], groupKey, ctx);
		}

		stringBuilder.AppendLine("}");
		return new SourceGenerationResult(groupKey, className, stringBuilder.ToString());
	}

	private static void GenerateInterfaceMethod(in StringBuilder stringBuilder, in int indent, in MethodDeclarationRecord method, in GeneratorExecutionContext ctx)
	{
		if (!method.TryCreateDeclaration(out var methodDeclaration))
		{
			ctx.ReportError("Cannot create the method name", method.ParamsType);
			return;
		}

		method.DeclarationString = methodDeclaration;

		stringBuilder
			.Indent(indent).AppendLine("/// <summary>")
			.Indent(indent).AppendFormat("/// Executes `{0}` with the specified <see cref=\"parameters\"/>", method.StoredProcedureName).AppendLine()
			.Indent(indent).AppendLine("/// </summary>")
			.Indent(indent).Append(methodDeclaration).AppendLine(";");
	}

	private static void GenerateClassMethod(in StringBuilder stringBuilder, int indent, in MethodDeclarationRecord method, in string groupKey, in GeneratorExecutionContext ctx)
	{
		if (method.ExecType == ExecType.Query)
			method.DeclarationString = AppendAsyncCancellation(method.DeclarationString);

		var connectionType = groupKey == "Auth"
			? groupKey
			: "Standard";

		stringBuilder
			.Indent(indent).Append("public async ").Append(method.DeclarationString).AppendLine()
			.Indent(indent++).AppendLine("{")
			.Indent(indent).AppendFormat("await using var {0} = new SqlConnection({1}.GetConnectionString(ConnectionType.{2}));", ConnectionVar, ConfigurationField, connectionType).AppendLine()
			.Indent(indent).AppendFormat("await {0}.OpenAsync(ct).ConfigureAwait(false);", ConnectionVar).AppendLine()
			.AppendLine();

		// Append temp tables
		if (method.TempTables.Count > 0)
		{
			var isAnyAppended = false;

			foreach (var (prop, tempTableName) in method.TempTables)
			{
				if (!prop.Type.TryGetGenericType(0, out var modelType))
					continue;

				const string commandVar = "tempTableCommand";
				isAnyAppended = true;

				var columnMapping = modelType.CreateColumnMapping(prop, ctx)
					.ToArray();

				var createTableSql = GenerateCreateTableSql(tempTableName, columnMapping);
				var insertSql = GenerateInsertSql(tempTableName, columnMapping);

				stringBuilder
					.Indent(indent).AppendFormat("if (parameters.{0}.Count > 0)", prop.Name).AppendLine()
					.Indent(indent++).AppendLine("{")
					.Indent(indent).AppendFormat("await using var {0} = new SqlCommand(\"{1}\", {2});", commandVar, createTableSql, ConnectionVar).AppendLine()
					.Indent(indent).AppendFormat("await {0}.ExecuteNonQueryAsync(ct).ConfigureAwait(false);", commandVar).AppendLine()
					.AppendLine()
					.Indent(indent).AppendFormat("{0}.CommandText = \"{1}\";", commandVar, insertSql).AppendLine();

				// Parameter declaration
				for (var i = 0; i < columnMapping.Length; i++)
				{
					stringBuilder
						.Indent(indent).AppendFormat("{0}.Parameters.Add(\"{1}\", SqlDbType.{2}", commandVar, columnMapping[i].ColumnPropertyOrThis(), columnMapping[i].ColumnType.DbType);

					if (columnMapping[i].ColumnType.Size.HasValue)
						stringBuilder.AppendFormat(", {0}", columnMapping[i].ColumnType.Size!.Value);

					stringBuilder.AppendLine(");");
				}

				stringBuilder
					.AppendLine()
					.Indent(indent).AppendFormat("for (var i = 0; i < parameters.{0}.Count; i++)", prop.Name).AppendLine()
					.Indent(indent++).AppendLine("{");

				// Parameter assignment
				for (var i = 0; i < columnMapping.Length; i++)
				{
					stringBuilder
						.Indent(indent).AppendFormat("{0}.Parameters[{1}].Value = parameters.{2}[i]", commandVar, i, columnMapping[i].PropertyName);

					if (!string.IsNullOrEmpty(columnMapping[i].ColumnProperty))
						stringBuilder.AppendFormat(".{0}", columnMapping[i].ColumnProperty);

					stringBuilder.AppendLine(";");
				}

				stringBuilder
					.Indent(indent).AppendFormat("await {0}.ExecuteNonQueryAsync(ct).ConfigureAwait(false);", commandVar).AppendLine()
					.Indent(--indent).AppendLine("}")
					.Indent(--indent).AppendLine("}");
			}

			if (isAnyAppended)
				stringBuilder.AppendLine();
		}

		stringBuilder
			.Indent(indent).AppendFormat("await using var {0} = new SqlCommand(\"{1}\", {2}) {{ CommandType = CommandType.StoredProcedure }};", CommandVar, method.StoredProcedureName, ConnectionVar).AppendLine();

		// Append proc parameters
		foreach (var (prop, paramName) in method.Parameters)
		{
			var value = "parameters." + prop.Name;
			if (prop.Type.NullableAnnotation == NullableAnnotation.Annotated)
				value = value + ".HasValue ? " + value + ".Value : DBNull.Value";

			stringBuilder
				.Indent(indent).AppendFormat("{0}.Parameters.AddWithValue(\"{1}\", {2});", CommandVar, paramName, value)
				.AppendLine();
		}

		// Append execution
		stringBuilder.AppendLine();
		switch (method.ExecType)
		{
			case ExecType.Query:
				GenerateQueryList(stringBuilder, method, indent);
				break;
			case ExecType.QueryFirst: break;
			case ExecType.Execute:
				GenerateExecute(stringBuilder, indent);
				break;
			default:
				ctx.ReportError("Unknown execution type", method.ParamsType);
				return;
		}

		stringBuilder
			.Indent(--indent).AppendLine("}");
	}

	private static void GenerateQueryList(in StringBuilder stringBuilder, MethodDeclarationRecord method, int indent)
	{
		stringBuilder
			.Indent(indent++).AppendFormat("await using var {0} = await {1}.ExecuteReaderAsync(ct)", ReaderVar, CommandVar).AppendLine()
			.Indent(indent--).AppendFormat(".ConfigureAwait(false);").AppendLine()
			.AppendLine();

		for (var i = 0; i < method.Results.Count; i++)
		{
			var (type, propertyName, keyProperty) = method.Results[i];
			var isNextDataSet = i != 0;

			if (isNextDataSet)
				stringBuilder.AppendLine();

			if (keyProperty != null)
			{
				stringBuilder
					.Indent(indent).AppendFormat("var grouping{0} = new LookUp<{1}, {2}>();", propertyName, keyProperty.Type, type.Type.GetNestedName())
					.AppendLine();
			}

			if (isNextDataSet)
			{
				stringBuilder
					.Indent(indent).AppendFormat("if (await {0}.NextResultAsync(ct).ConfigureAwait(false))", ReaderVar).AppendLine()
					.Indent(indent++).AppendLine("{");
			}

			stringBuilder
				.Indent(indent).AppendFormat("while (await {0}.ReadAsync(ct).ConfigureAwait(false))", ReaderVar).AppendLine()
				.Indent(indent++).AppendLine("{");

			var (vars, decl) = GenerateFieldAssignment(type, indent);
			stringBuilder.Append(vars);

			if (keyProperty != null)
			{
				const string itemVar = "item";

				stringBuilder
					.Indent(indent).AppendFormat("var {0} = ", itemVar)
					.Append(decl).AppendLine(";")
					.Indent(indent).AppendFormat("grouping{0}.Add({1}.{2}, {1});", propertyName, itemVar, keyProperty.Name).AppendLine();
			}
			else
			{
				stringBuilder
					.Indent(indent).Append("yield return ")
					.Append(decl).AppendLine(";");
			}

			stringBuilder
				.Indent(--indent).AppendLine("}");

			if (isNextDataSet)
			{
				stringBuilder
					.Indent(--indent).AppendLine("}");
			}
		}
	}

	private static void GenerateExecute(in StringBuilder stringBuilder, int indent)
	{
		stringBuilder
			.Indent(indent++).AppendFormat("await {0}.ExecuteNonQueryAsync(ct)", CommandVar).AppendLine()
			.Indent(indent).AppendLine(".ConfigureAwait(false);");
	}

	private static string AppendAsyncCancellation(in string declaration)
	{
		const string ct = "CancellationToken";
		var index = declaration.LastIndexOf(ct, StringComparison.CurrentCultureIgnoreCase);

		return index != -1
			? declaration.Insert(index, "[System.Runtime.CompilerServices.EnumeratorCancellation] ")
			: declaration;
	}

	private static string GenerateCreateTableSql(string tempTableName, IReadOnlyList<ColumnMappingRecord> props)
	{
		var stringBuilder = new StringBuilder()
			.AppendFormat("CREATE TABLE #{0} (", tempTableName);

		for (var i = 0; i < props.Count; i++)
		{
			if (i != 0)
				stringBuilder.Append(", ");

			stringBuilder.AppendFormat("{0} {1}", props[i].ColumnName, props[i].ColumnType);

			if (!string.IsNullOrEmpty(props[i].Collation))
				stringBuilder.AppendFormat(" COLLATE {0}", props[i].Collation);
		}

		return stringBuilder
			.Append(')')
			.ToString();
	}

	private static string GenerateInsertSql(string tempTableName, IReadOnlyList<ColumnMappingRecord> props)
	{
		var stringBuilder = new StringBuilder()
			.AppendFormat("INSERT #{0} (", tempTableName);

		// Columns
		for (var i = 0; i < props.Count; i++)
		{
			if (i != 0)
				stringBuilder.Append(", ");

			stringBuilder.Append(props[i].ColumnName);
		}

		stringBuilder.Append(") Values (");

		// Params
		for (var i = 0; i < props.Count; i++)
		{
			if (i != 0)
				stringBuilder.Append(", ");

			stringBuilder.AppendFormat("@{0}", props[i].ColumnPropertyOrThis());
		}

		return stringBuilder
			.Append(')')
			.ToString();
	}

	private static (string, string) GenerateFieldAssignment(in MethodDeclarationRecord.ResultType type, int indent)
	{
		var initialIndent = indent;

		var keyFields = type.Keys.Values
			.ToImmutableHashSet();

		var varsBuilder = new StringBuilder();

		var declBuilder = new StringBuilder()
			.AppendFormat("new {0}", type.Type.GetNestedName()).AppendLine()
			.Indent(indent++).AppendLine("{");

		int index = 0, fieldIndex = 0;
		foreach (var prop in type.Type.GetProperties(static x => x.DeclaredAccessibility == Accessibility.Public))
		{
			if (index != 0)
				declBuilder.AppendLine(",");

			declBuilder
				.Indent(indent).AppendFormat("{0} = ", prop.Name);

			if (prop.IsPropEnumerable())
			{
				if (!type.Keys.TryGetValue(prop.Name, out var propKey))
					throw new InvalidOperationException($"Cannot find the mapped key for `{prop.Name}`");

				declBuilder
					.AppendFormat("grouping{0}.GetItems({1})", prop.Name, propKey.ToPascalCase());
			}
			else
			{
				var propKey = keyFields
					.FirstOrDefault(x => x == prop.Name);

				if (propKey != null)
				{
					var varName = propKey.ToPascalCase();

					varsBuilder
						.Indent(initialIndent).AppendFormat("var {0} = {1}.Get{2}({3});", varName, ReaderVar, prop.Type.GetDatabaseReaderMethod(), fieldIndex)
						.AppendLine();

					declBuilder.Append(varName);
				}
				else
				{
					declBuilder
						.AppendFormat("{0}.Get{1}({2})", ReaderVar, prop.Type.GetDatabaseReaderMethod(), fieldIndex);
				}

				fieldIndex++;
			}

			index++;
		}

		declBuilder.AppendLine()
			.Indent(--indent).Append('}');

		return (varsBuilder.ToString(), declBuilder.ToString());
	}
}