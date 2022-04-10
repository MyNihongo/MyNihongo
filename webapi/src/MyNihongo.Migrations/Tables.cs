namespace MyNihongo.Migrations;

public static class Tables
{
	public static class Core
	{
		public const string Lang = "tblSysLanguage",
			User = "tblSysUser";
	}

	public static class Auth
	{
		public const string Connection = "tblAuthConnection",
			ConnectionToken = "tblAuthConnectionToken";
	}

	public static class Kanji
	{
		public const string MasterData = "tblKanjiMasterData",
			Reading = "tblKanjiReading",
			Meaning = "tblKanjiMeaning",
			UserEntry = "tblKanjiUserEntry";
	}

	public static class Grammar
	{
		public const string RuleMasterData = "tblGrammarRuleMasterData",
			RuleContent = "tblGrammarRuleContent",
			RuleSearchByKanji = "tblGrammarRuleSearchByKanji",
			RuleSearchByRomaji = "tblGrammarRuleSearchByRomaji",
			RuleSearchByLanguage = "tblGrammarRuleSearchByLanguage";
	}
}
