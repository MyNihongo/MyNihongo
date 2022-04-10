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

	public static class Auth
	{
		public static class Connection
		{
			public const string ConnectionId = "connectionID",
				UserId = User.UserId,
				IpAddress = "ipAddress",
				ClientInfo = "clientInfo",
				TicksLatestAccessed = "ticksLatestAccess";
		}

		public static class ConnectionToken
		{
			public const string TokenId = "tokenID",
				ConnectionId = Connection.ConnectionId,
				TicksValidTo = "ticksValidTo";
		}
	}

	public static class Kanji
	{
		public static class KanjiMasterData
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
			public const string KanjiId = KanjiMasterData.KanjiId,
				MainText = "main",
				SecondaryText = "secondary",
				SortingOrder = "sorting",
				ReadingType = "type",
				Romaji = "romaji";
		}

		public static class Meaning
		{
			public const string KanjiId = KanjiMasterData.KanjiId,
				LanguageId = Lang.LanguageId,
				Text = "text",
				SortingOrder = "sorting";
		}

		public static class UserEntry
		{
			public const string UserId = User.UserId,
				KanjiId = KanjiMasterData.KanjiId,
				FavouriteRating = "rating",
				Notes = "notes",
				Mark = "mark",
				IsDeleted = "isDeleted",
				TicksLatestAccess = "ticksLatestAccess",
				TicksModified = "ticksModified";
		}
	}

	public static class Grammar
	{
		public static class RuleMasterData
		{
			public const string GrammarRuleId = "grammarRuleId",
				SortingOrder = "sorting",
				Form = "form",
				JlptLevel = "jlptLevel",
				HashCode = "hash",
				Timestamp = "timestamp";
		}

		public static class RuleContent
		{
			public const string GrammarRuleId = RuleMasterData.GrammarRuleId,
				LanguageId = Lang.LanguageId,
				Header = "header",
				Content = "content";
		}

		public static class RuleSearchByKanji
		{
			public const string SearchText = "text",
				GrammarRuleId = RuleMasterData.GrammarRuleId;
		}

		public static class RuleSearchByRomaji
		{
			public const string SearchText = "text",
				GrammarRuleId = RuleMasterData.GrammarRuleId;
		}

		public static class RuleSearchByLanguage
		{
			public const string LanguageId = Lang.LanguageId,
				SearchText = "text",
				GrammarRuleId = RuleMasterData.GrammarRuleId;
		}
	}
}
