namespace MyNihongo.Migrations.Models;

[Table(Core.Lang)]
internal sealed record CoreLanguageRecord(Language Language, string Code)
{
	[Column(Lang.LanguageId)]
	public Language Language { get; } = Language;

	[Column(Lang.Code)]
	public string Code { get; } = Code;
}