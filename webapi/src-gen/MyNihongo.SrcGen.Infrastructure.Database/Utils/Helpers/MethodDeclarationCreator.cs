using System.Collections.Immutable;
using MyNihongo.SrcGen.Infrastructure.Database.Enums;
using MyNihongo.SrcGen.Infrastructure.Database.Models;
using MyNihongo.SrcGen.Infrastructure.Database.Resources;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Helpers;

internal static class MethodDeclarationCreator
{
	public static IReadOnlyList<MethodDeclarationRecord> CreateMethodDeclarations(this IReadOnlyList<TypeDeclarationSyntax> @this, Compilation compilation, in GeneratorExecutionContext ctx)
	{
		var list = new List<MethodDeclarationRecord>(@this.Count);

		foreach (var syntax in @this)
		{
			var typeSymbol = syntax.GetTypeSymbol(compilation);
			if (!typeSymbol.TryGetAttrValue(GeneratorConst.StoredProcedureContextAttributeName, 0, out string storedProcedureName))
			{
				ctx.ReportError($"Cannot find the name of a stored procedure for `{typeSymbol}`", typeSymbol);
				goto Next;
			}

			if (!typeSymbol.TryGetAttrValue(GeneratorConst.StoredProcedureContextAttributeName, 1, out INamedTypeSymbol? returnType))
			{
				ctx.ReportError($"Cannot find the return type for `{typeSymbol}`", typeSymbol);
				goto Next;
			}

			var isNullableReturnType = typeSymbol.GetAttrValue(GeneratorConst.StoredProcedureContextAttributeName, GeneratorConst.IsNullableProp, false);

			var paramsList = new List<MethodDeclarationRecord.Parameter>();
			var tempTableList = new List<MethodDeclarationRecord.Parameter>();
			foreach (var prop in typeSymbol.GetProperties(static x => x.SetMethod != null))
			{
				if (!prop.TryGetAttrValue(GeneratorConst.ParamAttributeName, 0, out string paramName))
				{
					if (prop.TryGetAttrValue(GeneratorConst.ParamTempTableAttributeName, 0, out paramName))
					{
						tempTableList.Add(new MethodDeclarationRecord.Parameter(prop, paramName));
						continue;
					}

					ctx.ReportError($"Attribute [{GeneratorConst.ParamAttributeName}] is missing for `{prop.Name}`", prop);
					goto Next;
				}

				paramsList.Add(new MethodDeclarationRecord.Parameter(prop, paramName));
			}

			var resultList = new List<MethodDeclarationRecord.Result>();
			if (returnType != null)
			{
				var itemReturnType = returnType.TypeArguments.Length == 1
					? (INamedTypeSymbol)returnType.TypeArguments[0]
					: returnType;

				var returnTypeProps = itemReturnType
					.GetProperties(static x => x.DeclaredAccessibility == Accessibility.Public)
					.ToDictionary(static x => x.Name);

				var keys = new Dictionary<string, string>();

				foreach (var prop in returnTypeProps.Values.Where(static x => x.IsPropEnumerable()))
				{
					var type = (INamedTypeSymbol)prop.Type;
					type = (INamedTypeSymbol)type.TypeArguments[0];

					var keyProp = type.GetProperties()
						.FirstOrDefault(x => returnTypeProps.ContainsKey(x.Name));

					if (keyProp == null)
						continue;

					keys.Add(prop.Name, keyProp.Name);

					var resultType = new MethodDeclarationRecord.ResultType(type, ImmutableDictionary<string, string>.Empty);
					var item = new MethodDeclarationRecord.Result(resultType, prop.Name, keyProp);
					resultList.Add(item);
				}

				var itemType = new MethodDeclarationRecord.ResultType(itemReturnType, keys);
				resultList.Add(new MethodDeclarationRecord.Result(itemType, "Items", null));
			}

			list.Add(new MethodDeclarationRecord(typeSymbol)
			{
				StoredProcedureName = storedProcedureName,
				ExecType = GetExecType(returnType),
				IsNullableReturnType = isNullableReturnType,
				Parameters = paramsList,
				TempTables = tempTableList,
				Results = resultList
			});

		Next:
			// ReSharper disable once RedundantJumpStatement
			continue;
		}

		return list;
	}

	private static ExecType GetExecType(ISymbol? returnType)
	{
		if (returnType == null)
			return ExecType.Execute;

		return returnType.Name == "IAsyncEnumerable"
			? ExecType.Query
			: ExecType.QueryFirst;
	}
}