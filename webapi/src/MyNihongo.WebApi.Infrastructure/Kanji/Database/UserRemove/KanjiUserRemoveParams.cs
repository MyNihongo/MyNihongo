namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiUserRemove", null)]
internal sealed record KanjiUserRemoveParams
{
	[Param("userID")]
	public long UserId { get; init; }

	[Param("kanjiID")]
	public int KanjiId { get; init; }

	[Param("ticksModified")]
	public long TicksModified { get; init; }
}