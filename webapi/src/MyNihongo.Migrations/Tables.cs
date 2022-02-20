namespace MyNihongo.Migrations;

public static class Tables
{
	public static class Core
	{
		public const string Lang = "tblSysLanguage",
			User = "tblSysUser";
	}

	public static class Kanji
	{
		public const string MasterData = "tblKanjiMasterData",
			Reading = "tblKanjiReading",
			Meaning = "tblKanjiMeaning",
			UserEntry = "tblKanjiUserEntry";
	}
}