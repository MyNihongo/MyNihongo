namespace MyNihongo.Migrations.Models.Options;

internal sealed record ConnectionString
{
	public string Value { get; set; } = string.Empty;
}