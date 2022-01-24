using System.Data;
using MyNihongo.Migrations.Models;
using MyNihongo.Migrations.Resources;

namespace MyNihongo.Migrations.Migrations;

[TimestampedMigration(2021, 1, 21, 0, 0)]
public sealed class InitialMigration : Migration
{
	public override void Up()
	{
		var actions = new Action<Migration>[]
		{
			CreateSystemLanguage,
			CreateKanjiMasterData,
			CreateKanjiReading,
			CreateKanjiMeaning
		};

		foreach (var action in actions)
			action(this);
	}

	public override void Down()
	{
		var tables = new[]
		{
			Kanji.Reading,
			Kanji.Meaning,
			Kanji.MasterData,
			Core.Lang
		};

		foreach (var table in tables)
			Delete.Table(table);
	}

	#region System

	private static void CreateSystemLanguage(Migration migration)
	{
		const string tblName = Core.Lang;

		migration.Create.Table(tblName)
			.WithColumn(Lang.LanguageId).AsByte().NotNullable().PrimaryKey()
			.WithColumn(Lang.Code).AsFixedLengthAnsiString(2);

		var items = new CoreLanguageRecord[]
		{
			new(Language.English, "en"),
			new(Language.Dutch, "nl"),
			new(Language.French, "fr"),
			new(Language.German, "de"),
			new(Language.Hungarian, "hu"),
			new(Language.Russian, "ru"),
			new(Language.Slovenian, "sl"),
			new(Language.Spanish, "es"),
			new(Language.Swedish, "sw"),
			new(Language.Portuguese, "pt")
		};

		migration.Insert.Rows(items);
	}

	#endregion

	#region Kanji

	private static void CreateKanjiMasterData(Migration migration)
	{
		const string tblName = Kanji.MasterData;

		migration.Create.Table(tblName)
			.WithColumn(MasterData.KanjiId).AsInt16().NotNullable().PrimaryKey()
			.WithColumn(MasterData.SortingOrder).AsInt16().NotNullable()
			.WithColumn(MasterData.Character).AsFixedLengthString(1, Collations.KanjiCharCollation).NotNullable().Unique()
			.WithColumn(MasterData.JlptLevel).AsByte().Nullable()
			.WithColumn(MasterData.HashCode).AsInt32().NotNullable()
			.WithColumn(MasterData.Timestamp).AsInt64().NotNullable();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(MasterData.JlptLevel).Descending();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(MasterData.SortingOrder).Ascending();
	}

	private static void CreateKanjiReading(Migration migration)
	{
		const string tblName = Kanji.Reading;

		migration.Create.Table(tblName)
			.WithColumn(Reading.KanjiId).AsInt16().NotNullable().ForeignKey(Kanji.MasterData, MasterData.KanjiId).OnDelete(Rule.Cascade)
			.WithColumn(Reading.MainText).AsString(Collations.KanjiKanaCollation).NotNullable()
			.WithColumn(Reading.SecondaryText).AsString(Collations.KanjiKanaCollation).NotNullable()
			.WithColumn(Reading.SorrintOrder).AsByte().NotNullable()
			.WithColumn(Reading.ReadingType).AsByte().NotNullable()
			.WithColumn(Reading.Romaji).AsString().NotNullable();

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(Reading.KanjiId, Reading.MainText, Reading.SecondaryText)
			.Clustered();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(Reading.SorrintOrder).Ascending();
	}

	public static void CreateKanjiMeaning(Migration migration)
	{
		const string tblName = Kanji.Meaning;

		migration.Create.Table(tblName)
			.WithColumn(Meaning.KanjiId).AsInt16().NotNullable().ForeignKey(Kanji.MasterData, MasterData.KanjiId).OnDelete(Rule.Cascade)
			.WithColumn(Meaning.LanguageId).AsByte().NotNullable().ForeignKey(Core.Lang, Lang.LanguageId)
			.WithColumn(Meaning.Text).AsString().NotNullable()
			.WithColumn(Meaning.SortingOrder).AsByte().NotNullable();

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(Meaning.KanjiId, Meaning.LanguageId, Meaning.Text)
			.Clustered();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(Meaning.SortingOrder);
	}

	#endregion
}