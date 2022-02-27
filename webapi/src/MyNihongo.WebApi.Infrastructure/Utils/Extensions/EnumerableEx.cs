using MyNihongo.WebApi.Infrastructure.Kanji;

namespace MyNihongo.WebApi.Infrastructure;

internal static class EnumerableEx
{
	public static IEnumerable<KanjiReading> ToKanjiReading(this IEnumerable<KanjiGetListResult.Reading> @this) =>
		@this.Select(static x => new KanjiReading
		{
			ReadingType = (KanjiReadingType)x.ReadingType,
			MainText = x.MainText,
			SecondaryText = x.SecondaryText,
			RomajiText = x.RomajiReading
		});

	public static IEnumerable<string> ToKanjiMeaning(this IEnumerable<KanjiGetListResult.Meaning> @this) =>
		@this.Select(static x => x.Text);
}