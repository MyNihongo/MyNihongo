using System.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class GeneratorExecutionContextEx
{
	public static Compilation AddSourceEx(in this GeneratorExecutionContext @this, string fileName, string source)
	{
		var srcText = SourceText.From(source, Encoding.UTF8);
		@this.AddSource(fileName, source);

		var options = (CSharpParseOptions)((CSharpCompilation)@this.Compilation).SyntaxTrees[0].Options;
		var compilation = @this.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(srcText, options));

		foreach (var diagnostic in compilation.GetDiagnostics())
		{
			if (diagnostic.Severity != DiagnosticSeverity.Error)
				continue;

			Debug.WriteLine(diagnostic.ToString());
		}

		return compilation;
	}

	public static void ReportError(in this GeneratorExecutionContext @this, string message, ISymbol typeSymbol)
	{
		var location = !typeSymbol.Locations.IsDefaultOrEmpty
			? typeSymbol.Locations[0]
			: Location.None;

		var descriptor = new DiagnosticDescriptor("MyNihongo.WebApi", "Generator error", message, "Error", DiagnosticSeverity.Error, true);
		var diagnostic = Diagnostic.Create(descriptor, location, DiagnosticSeverity.Error);

		@this.ReportDiagnostic(diagnostic);
	}
}