using System.Runtime.CompilerServices;
using MyNihongo.KanaConverter;
using MyNihongo.KanaDetector.Extensions;

namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal sealed class KanjiGetListRequestHandler : IStreamRequestHandler<KanjiGetListRequest, KanjiGetListResponse>
{
	private readonly IKanjiDatabaseService _kanjiDatabaseService;

	public KanjiGetListRequestHandler(IKanjiDatabaseService kanjiDatabaseService)
	{
		_kanjiDatabaseService = kanjiDatabaseService;
	}

	public async IAsyncEnumerable<KanjiGetListResponse> Handle(KanjiGetListRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (var kanji in GetKanjiAsync(request, cancellationToken).WithCancellation(cancellationToken))
		{
			var userData = kanji.FavouriteRating.HasValue
				? new KanjiGetListResponse.Types.UserData { FavouriteRating = kanji.FavouriteRating.Value.ToFavouriteRating() }
				: null;

			var readings = kanji.Readings
				.ToKanjiReading();

			var meanings = kanji.Meanings
				.ToKanjiMeaning();

			yield return new KanjiGetListResponse
			{
				KanjiId = kanji.KanjiId,
				SortingOrder = kanji.SortingOrder,
				Character = kanji.Character,
				JlptLevel = kanji.JlptLevel.ToJlptLevel(),
				UserData = userData,
				Readings = { readings },
				Meanings = { meanings }
			};
		}
	}

	private IAsyncEnumerable<KanjiGetListResult> GetKanjiAsync(KanjiGetListRequest request, CancellationToken ct = default)
	{
		request.SearchText = request.SearchText.TrimEx();

		bool byRomaji = true, byLanguage = true;

		if (request.SearchText.TryConvertToRomaji(TextUtils.StringBuilderPool.Get, out var romajiValue))
		{
			if (string.IsNullOrEmpty(request.SearchText))
			{
				var byJlpt = new KanjiGetListByJlptParams
				{
					JlptLevel = request.JlptLevel,
					Filter = request.Filter,
					Language = request.Language,
					UserId = request.UserId,
					PageIndex = request.PageIndex,
					PageSize = request.PageSize
				};

				return _kanjiDatabaseService.QueryByJlptAsync(byJlpt, ct);
			}

			request.SearchText = romajiValue;
			byLanguage = false;
		}
		else
		{
			const int maxKanjiLength = 25, minTextLength = 2;

			if (request.SearchText.Length < minTextLength)
				return AsyncEnumerable.Empty<KanjiGetListResult>();

			if (request.Language == Language.UndefinedLanguage)
				request.Language = Language.English;

			var kanji = new HashSet<char>();
			for (var i = 0; i < request.SearchText.Length; i++)
			{
				if (request.SearchText[i].IsKanji())
				{
					if (kanji.Count < maxKanjiLength)
						kanji.Add(request.SearchText[i]);
				}
				else
				{
					if (!request.SearchText[i].IsRomaji())
						byRomaji = false;

					if (!request.SearchText[i].IsLanguage(request.Language))
						byLanguage = false;
				}
			}

			if (kanji.Count > 0)
			{
				var chars = new char[kanji.Count];
				kanji.CopyTo(chars);

				var byChar = new KanjiGetListByCharParams
				{
					Characters = chars,
					JlptLevel = request.JlptLevel,
					Filter = request.Filter,
					Language = request.Language,
					UserId = request.UserId,
					PageIndex = request.PageIndex,
					PageSize = request.PageSize
				};

				return _kanjiDatabaseService.QueryByCharAsync(byChar, ct);
			}
		}

		if (byRomaji || byLanguage)
		{
			var byText = new KanjiGetListByTextParams
			{
				Text = request.SearchText,
				ByRomaji = byRomaji,
				ByLanguage = byLanguage,
				JlptLevel = request.JlptLevel,
				Filter = request.Filter,
				Language = request.Language,
				UserId = request.UserId,
				PageIndex = request.PageIndex,
				PageSize = request.PageSize
			};

			return _kanjiDatabaseService.QueryByTextAsync(byText, ct);
		}

		return AsyncEnumerable.Empty<KanjiGetListResult>();
	}
}