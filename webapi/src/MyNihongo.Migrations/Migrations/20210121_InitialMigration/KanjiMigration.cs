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
			.WithColumn(KanjiMasterData.KanjiId).AsInt16().NotNullable().PrimaryKey()
			.WithColumn(KanjiMasterData.SortingOrder).AsInt16().NotNullable()
			.WithColumn(KanjiMasterData.Character).AsFixedLengthString(1, Collations.KanjiCharCollation).NotNullable().Unique()
			.WithColumn(KanjiMasterData.JlptLevel).AsByte().Nullable()
			.WithColumn(KanjiMasterData.HashCode).AsInt32().NotNullable()
			.WithColumn(KanjiMasterData.Timestamp).AsInt64().NotNullable();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(KanjiMasterData.JlptLevel).Descending();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(KanjiMasterData.SortingOrder).Ascending();
	}

	private static void CreateKanjiReading(Migration migration)
	{
		const string tblName = Kanji.Reading;

		migration.Create.Table(tblName)
			.WithForeignKeyToKanji(Reading.KanjiId)
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
			.WithForeignKeyToKanji(Meaning.KanjiId)
			.WithForeignKeyToLanguage(Meaning.LanguageId)
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
			.WithForeignKeyToUser(UserEntry.UserId)
			.WithForeignKeyToKanji(UserEntry.KanjiId)
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
