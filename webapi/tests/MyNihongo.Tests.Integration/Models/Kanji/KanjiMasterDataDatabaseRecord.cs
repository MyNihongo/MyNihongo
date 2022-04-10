namespace MyNihongo.Tests.Integration.Models.Kanji;

[Table(Tables.Kanji.MasterData)]
public sealed record KanjiMasterDataDatabaseRecord
{
	[Column(KanjiMasterData.KanjiId, IsPrimaryKey = true)]
	public short KanjiId { get; init; }

	[Column(KanjiMasterData.SortingOrder)]
	public short SortingOrder { get; init; }

	[Column(KanjiMasterData.Character)]
	public char Character { get; init; }

	[Column(KanjiMasterData.JlptLevel)]
	public JlptLevel? JlptLevel { get; init; }

	[Column(KanjiMasterData.HashCode)]
	public int HashCode { get; init; }

	[Column(KanjiMasterData.Timestamp)]
	public long Timestamp { get; init; }
}
