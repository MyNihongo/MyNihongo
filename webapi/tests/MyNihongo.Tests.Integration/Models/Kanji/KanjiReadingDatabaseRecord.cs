namespace MyNihongo.Tests.Integration.Models.Kanji;

[Table(Tables.Kanji.Reading)]
public sealed record KanjiReadingDatabaseRecord
{
	[Column(Reading.KanjiId, IsPrimaryKey = true, PrimaryKeyOrder = 0)]
	public short KanjiId { get; init; }

	[Column(Reading.MainText, CanBeNull = false, IsPrimaryKey = true, PrimaryKeyOrder = 1, CreateFormat = $"{{0}} {{1}} COLLATE {Collations.KanjiKanaCollation} {{2}} {{3}}")]
	public string MainText { get; init; } = string.Empty;

	[Column(Reading.SecondaryText, CanBeNull = false, IsPrimaryKey = true, PrimaryKeyOrder = 2, CreateFormat = $"{{0}} {{1}} COLLATE {Collations.KanjiKanaCollation} {{2}} {{3}}")]
	public string SecondaryText { get; init; } = string.Empty;

	[Column(Reading.SortingOrder)]
	public byte SortingOrder { get; init; }

	[Column(Reading.ReadingType)]
	public KanjiReadingType ReadingType { get; init; }

	[Column(Reading.Romaji)]
	public string Romaji { get; init; } = string.Empty;
}