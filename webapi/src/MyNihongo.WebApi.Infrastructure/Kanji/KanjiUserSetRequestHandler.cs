namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed class KanjiUserSetRequestHandler : IRequestHandler<KanjiUserSetRequest, KanjiUserSetResponse>
{
	private readonly IClock _clock;
	private readonly IKanjiDatabaseService _kanjiDatabaseService;

	public KanjiUserSetRequestHandler(
		IClock clock,
		IKanjiDatabaseService kanjiDatabaseService)
	{
		_clock = clock;
		_kanjiDatabaseService = kanjiDatabaseService;
	}

	public async Task<KanjiUserSetResponse> Handle(KanjiUserSetRequest request, CancellationToken cancellationToken)
	{
		var parameters = new KanjiUserSetParams
		{
			UserId = request.UserId,
			KanjiId = request.KanjiId,
			FavouriteRating = request.FavouriteRating.GetByteRating(),
			Notes = request.Notes,
			Mark = request.Mark,
			TicksModified = _clock.GetTicksNow()
		};

		if (parameters.FavouriteRating.HasValue || parameters.Notes != null || parameters.Mark.HasValue)
		{
			await _kanjiDatabaseService.UserSetAsync(parameters, cancellationToken)
				.ConfigureAwait(false);
		}

		return new KanjiUserSetResponse();
	}
}