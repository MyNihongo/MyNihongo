namespace MyNihongo.SrcGen.Migrations.Utils.Extensions;

internal static class GeneratorExecutionContextEx
{
	public static void ReportError(in this GeneratorExecutionContext @this, string message, ISymbol typeSymbol)
	{
		var location = !typeSymbol.Locations.IsDefaultOrEmpty
			? typeSymbol.Locations[0]
			: Location.None;

		var descriptor = new DiagnosticDescriptor("MyNihongo.Migrator", "Generator error", message, "Error", DiagnosticSeverity.Error, true);
		var diagnostic = Diagnostic.Create(descriptor, location, DiagnosticSeverity.Error);

		@this.ReportDiagnostic(diagnostic);
	}
}