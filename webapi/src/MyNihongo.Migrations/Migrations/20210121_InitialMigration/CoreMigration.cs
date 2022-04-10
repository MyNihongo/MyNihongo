using MyNihongo.Migrations.Models;

namespace MyNihongo.Migrations.Migrations;

internal sealed class CoreMigration : IMigrationInternal
{
	public void Up(Migration migration)
	{
		var actions = new Action<Migration>[]
		{
			CreateCoreLanguage,
			CreateCoreUser
		};

		migration.InvokeAll(actions);
	}

	public void Down(Migration migration)
	{
		var tables = new[]
		{
			Core.User,
			Core.Lang
		};

		migration.DropAll(tables);
	}

	private static void CreateCoreLanguage(Migration migration)
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

	private static void CreateCoreUser(Migration migration)
	{
		const string tblName = Core.User;

		migration.Create.Table(tblName)
			.WithColumn(User.UserId).AsInt64().NotNullable().PrimaryKey().Identity()
			.WithColumn(User.EmailHash).AsBinary(64).NotNullable().Unique()
			.WithColumn(User.PasswordHash).AsBinary(64).NotNullable();
	}
}
