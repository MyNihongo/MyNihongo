namespace MyNihongo.WebApi.Infrastructure.Kanji;

[StoredProcedureContext("spKanjiQueryDetail", typeof(KanjiGetDetailResult))]
internal sealed record KanjiGetDetailParams
{
	private readonly Language _language = WebApiConst.DefaultLanguage;

	[Param("langID")]
	public Language Language
	{
		get => _language;
		init
		{
			if (value == Language.UndefinedLanguage)
				value = WebApiConst.DefaultLanguage;

			_language = value;
		}
	}

	[Param("userID")]
	public long? UserId { get; init; }

	[Param("kanjiID")]
	public int KanjiId { get; init; }
}