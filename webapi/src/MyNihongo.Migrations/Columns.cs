namespace MyNihongo.Migrations;

internal static class Columns
{
	public static class Core
	{
		public static class Lang
		{
			public const string LanguageId = "langID",
				Code = "code";
		}
	}

	public static class Kanji
	{
		public static class MasterData
		{
			public const string KanjiId = "kanjiID",
				SortingOrder = "sorting",
				Character = "char",
				JlptLevel = "jlptLevel",
				HashCode = "hash",
				Timestamp = "timestamp";
		}

		public static class Reading
		{
			public const string KanjiId = MasterData.KanjiId,
				MainText = "main",
				SecondaryText = "secondary",
				SorrintOrder = "sorting",
				ReadingType = "type",
				Romaji = "romaji";
		}

		public static class Meaning
		{
			public const string KanjiId = MasterData.KanjiId,
				LanguageId = Lang.LanguageId,
				Text = "text",
				SortingOrder = "sorting";
		}
	}
}