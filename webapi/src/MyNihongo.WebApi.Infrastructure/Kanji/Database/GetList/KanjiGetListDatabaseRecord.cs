namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed record KanjiGetListDatabaseRecord
{
	public short KanjiId { get; init; }

	public short SortingOrder { get; init; }

	public string Character { get; init; } = string.Empty;

	public byte? JlptLevel { get; init; }

	public byte? FavouriteRating { get; init; }

	public IReadOnlyList<Reading> Readings { get; init; } = Array.Empty<Reading>();

	public IReadOnlyList<Meaning> Meanings { get; init; } = Array.Empty<Meaning>();

	public sealed record Reading
	{
		public short KanjiId { get; init; }

		public byte ReadingType { get; init; }

		public string MainText { get; init; } = string.Empty;

		public string SecondaryText { get; init; } = string.Empty;

		public string RomajiReading { get; init; } = string.Empty;
	}

	public sealed record Meaning
	{
		public short KanjiId { get; init; }

		public string Text { get; init; } = string.Empty;
	}
}
