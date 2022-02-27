namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed class KanjiUserRemoveRequestHandler : IRequestHandler<KanjiUserRemoveRequest, KanjiUserRemoveResponse>
{
	private readonly IClock _clock;
	private readonly IKanjiDatabaseService _kanjiDatabaseService;

	public KanjiUserRemoveRequestHandler(
		IClock clock,
		IKanjiDatabaseService kanjiDatabaseService)
	{
		_clock = clock;
		_kanjiDatabaseService = kanjiDatabaseService;
	}

	public async Task<KanjiUserRemoveResponse> Handle(KanjiUserRemoveRequest request, CancellationToken cancellationToken)
	{
		var parameters = new KanjiUserRemoveParams
		{
			UserId = request.UserId,
			KanjiId = request.KanjiId,
			TicksModified = _clock.GetTicksNow()
		};

		await _kanjiDatabaseService.UserRemoveAsync(parameters, cancellationToken)
			.ConfigureAwait(false);

		return new KanjiUserRemoveResponse();
	}
}