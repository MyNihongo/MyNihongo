namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed class KanjiUserAddRequestHandler : IRequestHandler<KanjiUserAddRequest, KanjiUserAddResponse>
{
	private readonly IClock _clock;
	private readonly IKanjiDatabaseService _kanjiDatabaseService;

	public KanjiUserAddRequestHandler(
		IClock clock,
		IKanjiDatabaseService kanjiDatabaseService)
	{
		_clock = clock;
		_kanjiDatabaseService = kanjiDatabaseService;
	}

	public async Task<KanjiUserAddResponse> Handle(KanjiUserAddRequest request, CancellationToken cancellationToken)
	{
		var parameters = new KanjiUserAddParams
		{
			UserId = request.UserId,
			KanjiId = request.KanjiId,
			TicksModified = _clock.GetTicksNow()
		};

		var userData = await _kanjiDatabaseService.UserAddAsync(parameters, cancellationToken)
			.ConfigureAwait(false);

		return new KanjiUserAddResponse
		{
			FavouriteRating = userData.FavouriteRating.ToFavouriteRating(),
			Notes = userData.Notes,
			Mark = userData.Mark
		};
	}
}