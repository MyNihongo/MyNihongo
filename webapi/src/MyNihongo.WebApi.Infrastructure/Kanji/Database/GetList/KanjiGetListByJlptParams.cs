namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiQueryByJlpt", typeof(IAsyncEnumerable<KanjiGetListResult>))]
internal sealed record KanjiGetListByJlptParams : KanjiGetListBaseParams;
