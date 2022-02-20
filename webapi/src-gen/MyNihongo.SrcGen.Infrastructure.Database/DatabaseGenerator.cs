using MyNihongo.SrcGen.Infrastructure.Database.Models;
using MyNihongo.SrcGen.Infrastructure.Database.Utils;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;
using MyNihongo.SrcGen.Infrastructure.Database.Utils.Helpers;

namespace MyNihongo.SrcGen.Infrastructure.Database;

[Generator]
public sealed class DatabaseGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context)
	{
#if DEBUG
		//if (!System.Diagnostics.Debugger.IsAttached)
		//	System.Diagnostics.Debugger.Launch();
#endif

		context.RegisterForSyntaxNotifications(() => new DatabaseSyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext context)
	{
		var compilation = context.Compilation;
		var source = compilation.GenerateAttributes();
		compilation = context.AddSourceEx("Attributes.g.cs", source);

		if (context.SyntaxReceiver is not DatabaseSyntaxReceiver syntaxReceiver)
			return;

		var classDeclarations = new List<ClassDeclarationRecord>(syntaxReceiver.CandidatesCount);
		foreach (var groupKey in syntaxReceiver.GetGroupKeys())
		{
			var typeDeclarations = syntaxReceiver[groupKey].CreateMethodDeclarations(compilation, context);
			var (@namespace, className, declaration) = compilation.GenerateSource(context, groupKey, typeDeclarations);
			context.AddSource($"{className}.g.cs", declaration);

			classDeclarations.Add(new ClassDeclarationRecord
			{
				Namespace = @namespace,
				ClassName = className
			});
		}

		source = compilation.GenerateServiceRegistration(classDeclarations);
		context.AddSource("ServiceCollectionEx.g.cs", source);
	}
}