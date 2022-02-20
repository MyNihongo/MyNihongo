namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiQueryByJlpt", typeof(IAsyncEnumerable<KanjiGetListDatabaseRecord>))]
internal sealed record KanjiGetListByJlptParams : KanjiGetListBaseParams;
