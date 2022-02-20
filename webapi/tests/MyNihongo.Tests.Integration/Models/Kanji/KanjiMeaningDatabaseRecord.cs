namespace MyNihongo.Tests.Integration.Models.Kanji;

[Table(Tables.Kanji.Meaning)]
public sealed record KanjiMeaningDatabaseRecord
{
	[Column(Meaning.KanjiId, IsPrimaryKey = true, PrimaryKeyOrder = 0)]
	public short KanjiId { get; init; }

	[Column(Meaning.LanguageId, IsPrimaryKey = true, PrimaryKeyOrder = 1)]
	public Language Language { get; init; }

	[Column(Meaning.Text, CanBeNull = false, IsPrimaryKey = true, PrimaryKeyOrder = 2)]
	public string Text { get; init; } = string.Empty;

	[Column(Meaning.SortingOrder)]
	public byte SortingOrder { get; init; }
}