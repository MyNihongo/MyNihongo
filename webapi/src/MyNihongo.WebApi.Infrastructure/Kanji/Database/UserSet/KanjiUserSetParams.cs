namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiUserSet", null)]
internal sealed record KanjiUserSetParams
{
	[Param("userID")]
	public long UserId { get; init; }

	[Param("kanjiID")]
	public int KanjiId { get; init; }

	[Param("rating")]
	public byte? FavouriteRating { get; init; }

	[Param("notes")]
	public string? Notes { get; init; }

	[Param("mark")]
	public byte? Mark { get; init; }

	[Param("ticksModified")]
	public long TicksModified { get; init; }
}