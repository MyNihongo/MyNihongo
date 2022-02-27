namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiQueryByText", typeof(IAsyncEnumerable<KanjiGetListResult>))]
internal sealed record KanjiGetListByTextParams : KanjiGetListBaseParams
{
	[Param("text")]
	public string Text { get; init; } = string.Empty;

	[Param("byRomaji")]
	public bool ByRomaji { get; init; }

	[Param("byLanguage")]
	public bool ByLanguage { get; init; }
}