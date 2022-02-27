namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed record KanjiGetDetailResult
{
	public short KanjiId { get; init; }

	public string Character { get; init; } = string.Empty;

	public byte? JlptLevel { get; init; }

	public byte? FavouriteRating { get; init; }
	
	public string? Notes { get; init; }
	
	public byte? Mark { get; init; }

	public IReadOnlyList<KanjiGetListResult.Reading> Readings { get; init; } = Array.Empty<KanjiGetListResult.Reading>();

	public IReadOnlyList<KanjiGetListResult.Meaning> Meanings { get; init; } = Array.Empty<KanjiGetListResult.Meaning>();
}