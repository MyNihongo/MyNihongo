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
				? new KanjiGetListResponse.Types.UserData { FavouriteRating = GetFavouriteRating(kanji.FavouriteRating.Value) }
				: null;

			var readings = kanji.Readings
				.Select(static x => new KanjiReading
				{
					ReadingType = (KanjiReadingType)x.ReadingType,
					MainText = x.MainText,
					SecondaryText = x.SecondaryText,
					RomajiText = x.RomajiReading
				});

			var meanings = kanji.Meanings
				.Select(static x => x.Text);

			yield return new KanjiGetListResponse
			{
				KanjiId = kanji.KanjiId,
				SortingOrder = kanji.SortingOrder,
				Character = kanji.Character,
				JlptLevel = kanji.JlptLevel.HasValue ? (JlptLevel)kanji.JlptLevel.Value : JlptLevel.UndefinedJlptLevel,
				UserData = userData,
				Readings = { readings },
				Meanings = { meanings }
			};
		}

		static double GetFavouriteRating(in byte favouriteRating) =>
			Math.Round(favouriteRating / 10d, 1);
	}

	private IAsyncEnumerable<KanjiGetListDatabaseRecord> GetKanjiAsync(KanjiGetListRequest request, CancellationToken ct = default)
	{
		request.SearchText = request.SearchText.TrimEx();

		bool byRomaji = true, byLanguage = true;

		if (request.SearchText.TryConvertToRomaji(TextUtils.StringBuilderPool.Get, out var romajiValue))
		{
			if (string.IsNullOrEmpty(request.SearchText))
			{
				var byJlpt = new KanjiGetListByJlptParams
				{
					JlptLevel = request.JlptLevel.NullIfDefault(),
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
				return AsyncEnumerable.Empty<KanjiGetListDatabaseRecord>();

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
					JlptLevel = request.JlptLevel.NullIfDefault(),
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
				JlptLevel = request.JlptLevel.NullIfDefault(),
				Filter = request.Filter,
				Language = request.Language,
				UserId = request.UserId,
				PageIndex = request.PageIndex,
				PageSize = request.PageSize
			};

			return _kanjiDatabaseService.QueryByTextAsync(byText, ct);
		}

		return AsyncEnumerable.Empty<KanjiGetListDatabaseRecord>();
	}
}