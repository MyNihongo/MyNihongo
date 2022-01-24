namespace MyNihongo.Migrations;

internal static class Tables
{
	public static class Core
	{
		public const string Lang = "tblSysLanguage";
	}

	public static class Kanji
	{
		public const string MasterData = "tblKanjiMasterData",
			Reading = "tblKanjiReading",
			Meaning = "tblKanjiMeaning";
	}
}