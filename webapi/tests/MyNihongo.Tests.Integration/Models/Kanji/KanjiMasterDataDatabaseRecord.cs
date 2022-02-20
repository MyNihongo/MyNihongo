namespace MyNihongo.Tests.Integration.Models.Kanji;

[Table(Tables.Kanji.MasterData)]
public sealed record KanjiMasterDataDatabaseRecord
{
	[Column(MasterData.KanjiId, IsPrimaryKey = true)]
	public short KanjiId { get; init; }

	[Column(MasterData.SortingOrder)]
	public short SortingOrder { get; init; }

	[Column(MasterData.Character)]
	public char Character { get; init; }

	[Column(MasterData.JlptLevel)]
	public JlptLevel? JlptLevel { get; init; }

	[Column(MasterData.HashCode)]
	public int HashCode { get; init; }

	[Column(MasterData.Timestamp)]
	public long Timestamp { get; init; }
}