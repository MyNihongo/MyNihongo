namespace MyNihongo.SrcGen.Infrastructure.Database.Models;

internal sealed record ClassDeclarationRecord
{
	public string Namespace { get; set; } = string.Empty;

	public string ClassName { get; set; } = string.Empty;
}