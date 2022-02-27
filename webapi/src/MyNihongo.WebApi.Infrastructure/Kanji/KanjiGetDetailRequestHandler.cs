namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed class KanjiGetDetailRequestHandler : IRequestHandler<KanjiGetDetailRequest, KanjiGetDetailResponse>
{
	private readonly IKanjiDatabaseService _kanjiDatabaseService;

	public KanjiGetDetailRequestHandler(IKanjiDatabaseService kanjiDatabaseService)
	{
		_kanjiDatabaseService = kanjiDatabaseService;
	}

	public async Task<KanjiGetDetailResponse> Handle(KanjiGetDetailRequest request, CancellationToken cancellationToken)
	{
		var parameters = new KanjiGetDetailParams
		{
			Language = request.Language,
			UserId = request.UserId,
			KanjiId = request.KanjiId
		};

		var kanji = await _kanjiDatabaseService.QueryDetailAsync(parameters, cancellationToken)
			.ConfigureAwait(false);

		var userData = kanji.FavouriteRating.HasValue && kanji.Mark.HasValue
			? new KanjiGetDetailResponse.Types.UserData { FavouriteRating = kanji.FavouriteRating.Value.ToFavouriteRating(), Mark = kanji.Mark.Value, Notes = kanji.Notes }
			: null;

		var readings = kanji.Readings
			.ToKanjiReading();

		var meanings = kanji.Meanings
			.ToKanjiMeaning();

		return new KanjiGetDetailResponse
		{
			Character = kanji.Character,
			JlptLevel = kanji.JlptLevel.ToJlptLevel(),
			UserData = userData,
			Readings = { readings },
			Meanings = { meanings }
		};
	}
}