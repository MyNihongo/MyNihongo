using MyNihongo.SrcGen.Infrastructure.Database.Resources;

namespace MyNihongo.SrcGen.Infrastructure.Database;

internal sealed class DatabaseSyntaxReceiver : ISyntaxReceiver
{
	private readonly Dictionary<string, List<TypeDeclarationSyntax>> _candidates = new();

	public int CandidatesCount => _candidates.Count;

	public IReadOnlyList<TypeDeclarationSyntax> this[string key] => _candidates[key];

	public IEnumerable<string> GetGroupKeys() =>
		_candidates.Keys;

	public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
	{
		if (syntaxNode is not AttributeSyntax { Name: IdentifierNameSyntax { Identifier.ValueText: GeneratorConst.StoredProcedureContextAttributeName } } attributeSyntax)
			return;

		if (syntaxNode.Parent?.Parent is not TypeDeclarationSyntax typeSyntax)
			return;

		var storedProcedureParam = attributeSyntax.DescendantNodes()
			.OfType<AttributeArgumentSyntax>()
			.FirstOrDefault()
			?.Expression as LiteralExpressionSyntax;

		if (!TryGetGroupingKey(storedProcedureParam?.Token.ValueText, out var groupingKey))
			return;

		if (!_candidates.TryGetValue(groupingKey, out var list))
		{
			list = new List<TypeDeclarationSyntax>(1);
			_candidates[groupingKey] = list;
		}

		list.Add(typeSyntax);
	}

	private static bool TryGetGroupingKey(string? storedProcedureName, out string groupingKey)
	{
		const string prefix = "sp";

		groupingKey = string.Empty;
		storedProcedureName ??= string.Empty;

		if (!storedProcedureName.StartsWith(prefix))
			return false;

		for (var i = prefix.Length + 1; i < storedProcedureName.Length; i++)
		{
			if (!char.IsUpper(storedProcedureName[i]))
				continue;

			groupingKey = storedProcedureName.Substring(prefix.Length, i - prefix.Length);
			return true;
		}

		return false;
	}
}