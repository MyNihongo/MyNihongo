namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiUserAdd", typeof(KanjiUserAddResult))]
internal sealed record KanjiUserAddParams
{
	[Param("userID")]
	public long UserId { get; init; }

	[Param("kanjiID")]
	public int KanjiId { get; init; }

	[Param("ticksModified")]
	public long TicksModified { get; init; }
}