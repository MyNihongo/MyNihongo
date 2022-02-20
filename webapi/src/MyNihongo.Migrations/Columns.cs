namespace MyNihongo.Migrations;

public static class Columns
{
	public static class Core
	{
		public static class Lang
		{
			public const string LanguageId = "langID",
				Code = "code";
		}

		public static class User
		{
			public const string UserId = "userID",
				EmailHash = "emailHash",
				PasswordHash = "passwordHash";
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
				SortingOrder = "sorting",
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

		public static class UserEntry
		{
			public const string UserId = User.UserId,
				KanjiId = MasterData.KanjiId,
				FavouriteRating = "rating",
				Notes = "notes",
				Mark = "mark",
				IsDeleted = "isDeleted",
				TicksLastAccessed = "ticksLastAccessed",
				TicksModified = "ticksModified";
		}
	}
}