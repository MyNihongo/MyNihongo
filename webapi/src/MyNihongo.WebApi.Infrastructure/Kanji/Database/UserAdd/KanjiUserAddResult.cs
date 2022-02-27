namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed record KanjiUserAddResult
{
	public byte FavouriteRating { get; init; }

	public string Notes { get; init; } = string.Empty;

	public byte Mark { get; init; }
}