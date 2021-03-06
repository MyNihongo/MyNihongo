using System.Data;
using MyNihongo.Migrations.Utils.Extensions;

namespace MyNihongo.Migrations.Migrations;

internal sealed class KanjiMigration : IMigrationInternal
{
	public void Up(Migration migration)
	{
		var actions = new Action<Migration>[]
		{
			CreateKanjiMasterData,
			CreateKanjiReading,
			CreateKanjiMeaning,
			CreateKanjiUserEntry
		};

		migration.InvokeAll(actions);
	}

	public void Down(Migration migration)
	{
		var tables = new[]
		{
			Kanji.UserEntry,
			Kanji.Reading,
			Kanji.Meaning,
			Kanji.MasterData
		};

		migration.DropAll(tables);
	}

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
			.WithColumn(Reading.SortingOrder).AsByte().NotNullable()
			.WithColumn(Reading.ReadingType).AsByte().NotNullable()
			.WithColumn(Reading.Romaji).AsString().NotNullable();

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(Reading.KanjiId, Reading.MainText, Reading.SecondaryText)
			.Clustered();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(Reading.SortingOrder).Ascending();
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

	public static void CreateKanjiUserEntry(Migration migration)
	{
		const string tblName = Kanji.UserEntry;

		migration.Create.Table(tblName)
			.WithColumn(UserEntry.UserId).AsInt64().NotNullable().ForeignKey(Core.User, User.UserId)
			.WithColumn(UserEntry.KanjiId).AsInt16().NotNullable().ForeignKey(Kanji.MasterData, MasterData.KanjiId)
			.WithColumn(UserEntry.FavouriteRating).AsByte().NotNullable().WithDefaultValue(0)
			.WithColumn(UserEntry.Notes).AsString(int.MaxValue).NotNullable().WithDefaultValue(string.Empty)
			.WithColumn(UserEntry.Mark).AsByte().NotNullable().WithDefaultValue(0)
			.WithColumn(UserEntry.IsDeleted).AsBoolean().NotNullable().WithDefaultValue(false)
			.WithColumn(UserEntry.TicksLatestAccess).AsInt64().NotNullable().WithDefaultValue(0)
			.WithColumn(UserEntry.TicksModified).AsInt64().NotNullable();

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(UserEntry.UserId, UserEntry.KanjiId);

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(UserEntry.IsDeleted);

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(UserEntry.FavouriteRating);
	}
}