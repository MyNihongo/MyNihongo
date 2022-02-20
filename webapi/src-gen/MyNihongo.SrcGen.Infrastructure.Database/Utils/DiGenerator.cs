using MyNihongo.SrcGen.Infrastructure.Database.Models;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils;

internal static class DiGenerator
{
	public static string GenerateServiceRegistration(this Compilation @this, IReadOnlyList<ClassDeclarationRecord> classDeclarations)
	{
		var stringBuilder = new StringBuilder()
			.AppendNamespace(@this)
			.AppendLine("internal static class ServiceCollectionForDatabaseEx")
			.AppendLine("{")
			.AppendLine("\tpublic static IServiceCollection AddDatabaseServices(this IServiceCollection @this) =>")
			.Append("\t\t@this");

		for (var i = 0; i < classDeclarations.Count; i++)
		{
			stringBuilder
				.AppendLine()
				.AppendFormat("\t\t\t.AddSingleton<{0}.I{1}, {0}.{1}>()", classDeclarations[i].Namespace, classDeclarations[i].ClassName);
		}

		return stringBuilder
			.AppendLine(";")
			.AppendLine("}")
			.ToString();
	}
}