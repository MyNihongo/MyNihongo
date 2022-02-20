using MyNihongo.SrcGen.Infrastructure.Database.Models;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class MethodDeclarationRecordEx
{
	public static bool TryCreateDeclaration(this MethodDeclarationRecord @this, out string value)
	{
		if (!TryGetMethodName(@this.StoredProcedureName, out var methodName))
		{
			value = string.Empty;
			return false;
		}

		const string asyncSuffix = "Async";

		var stringBuilder = new StringBuilder()
			.Append(CreateReturnType(@this.Results))
			.AppendFormat(" {0}", methodName);

		if (!methodName.EndsWith(asyncSuffix))
			stringBuilder.Append(asyncSuffix);

		value = stringBuilder
			.AppendFormat("({0} parameters, CancellationToken ct = default)", @this.ParamsType.Name)
			.ToString();

		return true;

		static string CreateReturnType(IReadOnlyList<MethodDeclarationRecord.Result> returnTypes)
		{
			if (returnTypes.Count == 0)
				return "Task";
			if (returnTypes.Count == 1)
				return $"Task<{returnTypes[0].Type.Type.Name}>";

			var itemType = returnTypes[returnTypes.Count - 1].Type.Type.Name;
			return $"IAsyncEnumerable<{itemType}>";
		}

		static bool TryGetMethodName(string storedProcedureName, out string value)
		{
			for (int i = 0, upperCaseCount = 0; i < storedProcedureName.Length; i++)
			{
				if (!char.IsUpper(storedProcedureName[i]) || ++upperCaseCount < 2)
					continue;

				value = storedProcedureName.Substring(i);
				return true;
			}

			value = string.Empty;
			return false;
		}
	}
}