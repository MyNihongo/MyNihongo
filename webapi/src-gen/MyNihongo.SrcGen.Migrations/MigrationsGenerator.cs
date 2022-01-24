using MyNihongo.SrcGen.Migrations.Utils;

namespace MyNihongo.SrcGen.Migrations;

[Generator]
public sealed class MigrationsGenerator : ISourceGenerator
{
	public void Initialize(GeneratorInitializationContext context)
	{
#if DEBUG
		//if (!System.Diagnostics.Debugger.IsAttached)
		//	System.Diagnostics.Debugger.Launch();
#endif

		context.RegisterForSyntaxNotifications(() => new MigrationsSyntaxReceiver());
	}

	public void Execute(GeneratorExecutionContext context)
	{
		if (context.SyntaxReceiver is not MigrationsSyntaxReceiver {Candidates.Count: > 0} syntaxReceiver)
			return;

		var source = context.GenerateSource(syntaxReceiver.Candidates);
		context.AddSource("Migrations.g.cs", source);
	}
}