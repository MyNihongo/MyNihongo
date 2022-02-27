using MyNihongo.SrcGen.Infrastructure.Database.Enums;
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
			.Append(CreateReturnType(@this))
			.AppendFormat(" {0}", methodName);

		if (!methodName.EndsWith(asyncSuffix))
			stringBuilder.Append(asyncSuffix);

		value = stringBuilder
			.AppendFormat("({0} parameters, CancellationToken ct = default)", @this.ParamsType.Name)
			.ToString();

		return true;

		static string CreateReturnType(MethodDeclarationRecord @this)
		{
			switch (@this.ExecType)
			{
				case ExecType.Query:
					return $"IAsyncEnumerable<{GetReturnTypeName(@this)}>";
				case ExecType.QueryFirst:
					return $"Task<{GetReturnTypeName(@this)}>";
				case ExecType.Execute:
					return "Task";
			}

			// return something stupid so that the compiler warns us
			return "void";
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

	public static string? TryGetReturnTypeName(this MethodDeclarationRecord @this) =>
		@this.Results.Count > 0
			? @this.GetReturnTypeName()
			: null;

	private static string GetReturnTypeName(this MethodDeclarationRecord @this) =>
		@this.Results[@this.Results.Count - 1].Type.Value.Name;
}