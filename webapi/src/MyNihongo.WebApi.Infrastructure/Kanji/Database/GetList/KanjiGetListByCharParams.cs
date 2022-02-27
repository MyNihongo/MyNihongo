namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiQueryByChar", typeof(IAsyncEnumerable<KanjiGetListResult>))]
internal sealed record KanjiGetListByCharParams : KanjiGetListBaseParams
{
	[ParamTempTable("tmpChar", Collation = Database.Abstractions.Collations.KanjiCharCollation)]
	public IReadOnlyList<char> Characters { get; init; } = Array.Empty<char>();
}