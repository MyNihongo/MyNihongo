namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiQueryByChar", typeof(IAsyncEnumerable<KanjiGetListDatabaseRecord>))]
internal sealed record KanjiGetListByCharParams : KanjiGetListBaseParams
{
	[ParamTempTable("tmpChar", Collation = MyNihongo.Database.Abstractions.Collations.KanjiCharCollation)]
	public IReadOnlyList<char> Characters { get; init; } = Array.Empty<char>();
}