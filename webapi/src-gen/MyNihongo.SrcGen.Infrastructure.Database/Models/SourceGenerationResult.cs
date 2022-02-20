namespace MyNihongo.SrcGen.Infrastructure.Database.Models;

internal readonly ref struct SourceGenerationResult
{
	public SourceGenerationResult(string @namespace, string className, string declaration)
	{
		Namespace = @namespace;
		ClassName = className;
		Declaration = declaration;
	}

	public string Namespace { get; }

	public string ClassName { get; }

	public string Declaration { get; }

	public void Deconstruct(out string @namespace, out string className, out string declaration)
	{
		@namespace = Namespace;
		className = ClassName;
		declaration = Declaration;
	}
}