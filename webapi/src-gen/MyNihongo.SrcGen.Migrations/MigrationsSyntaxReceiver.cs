using MyNihongo.SrcGen.Migrations.Resources;

namespace MyNihongo.SrcGen.Migrations;

internal sealed class MigrationsSyntaxReceiver : ISyntaxReceiver
{
	private readonly List<TypeDeclarationSyntax> _candidates = new();

	public IReadOnlyList<TypeDeclarationSyntax> Candidates => _candidates;

	public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
	{
		if (syntaxNode is not TypeDeclarationSyntax typeSyntax)
			return;

		var hasTable = typeSyntax.AttributeLists
			.SelectMany(x => x.Attributes)
			.Any(x => x.Name.ToString() == GeneratorConst.TableAttributeName);

		if (hasTable)
			_candidates.Add(typeSyntax);
	}
}