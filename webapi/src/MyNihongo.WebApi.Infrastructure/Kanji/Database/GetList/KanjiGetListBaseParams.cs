namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal abstract record KanjiGetListBaseParams : PaginationBaseParams
{
	private readonly JlptLevel? _jlptLevel;
	private readonly Language _language = WebApiConst.DefaultLanguage;

	[Param("jlptLevel")]
	public JlptLevel? JlptLevel
	{
		get => _jlptLevel;
		init => _jlptLevel = value != Infrastructure.JlptLevel.UndefinedJlptLevel ? value : null;
	}

	[Param("filterID")]
	public KanjiGetListRequest.Types.Filter Filter { get; init; }

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
}