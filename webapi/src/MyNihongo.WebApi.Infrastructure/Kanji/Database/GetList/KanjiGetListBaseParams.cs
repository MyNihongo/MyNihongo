namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal abstract record KanjiGetListBaseParams : PaginationBaseParams
{
	private readonly Language _language;

	[Param("jlptLevel")]
	public JlptLevel? JlptLevel { get; init; }

	[Param("filterID")]
	public KanjiGetListRequest.Types.Filter Filter { get; init; }

	[Param("langID")]
	public Language Language
	{
		get => _language;
		init
		{
			if (value == Language.UndefinedLanguage)
				value = Language.English;

			_language = value;
		}
	}

	[Param("userID")]
	public long? UserId { get; init; }
}