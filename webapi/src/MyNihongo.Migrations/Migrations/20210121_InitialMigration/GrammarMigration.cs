namespace MyNihongo.Migrations.Migrations;

internal sealed class GrammarMigration : IMigrationInternal
{
	public void Up(Migration migration)
	{
		var actions = new Action<Migration>[]
		{
			CreateRuleMasterData,
			CreateRuleContent,
			CreateRuleSearchByKanji,
			CreateRuleSearchByRomaji,
			CreateRuleSearchByLanguage
		};

		migration.InvokeAll(actions);
	}

	public void Down(Migration migration)
	{
		var tables = new[]
		{
			Grammar.RuleSearchByLanguage,
			Grammar.RuleSearchByRomaji,
			Grammar.RuleSearchByKanji,
			Grammar.RuleContent,
			Grammar.RuleMasterData
		};

		migration.DropAll(tables);
	}

	private static void CreateRuleMasterData(Migration migration)
	{
		const string tblName = Grammar.RuleMasterData;

		migration.Create.Table(tblName)
			.WithColumn(RuleMasterData.GrammarRuleId).AsInt16().PrimaryKey()
			.WithColumn(RuleMasterData.SortingOrder).AsInt16().NotNullable()
			.WithColumn(RuleMasterData.Form).AsString(Collations.KanjiKanaCollation).NotNullable()
			.WithColumn(RuleMasterData.JlptLevel).AsByte().NotNullable()
			.WithColumn(RuleMasterData.HashCode).AsInt32().NotNullable()
			.WithColumn(RuleMasterData.Timestamp).AsInt64().NotNullable();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(RuleMasterData.JlptLevel).Descending();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(RuleMasterData.SortingOrder).Ascending();
	}

	private static void CreateRuleContent(Migration migration)
	{
		const string tblName = Grammar.RuleContent;

		migration.Create.Table(tblName)
			.WithForeignKeyToGrammarRule(RuleContent.GrammarRuleId)
			.WithForeignKeyToLanguage(RuleContent.LanguageId)
			.WithColumn(RuleContent.Header).AsString(int.MaxValue, Collations.KanjiKanaCollation).NotNullable()
			.WithColumn(RuleContent.Content).AsString(int.MaxValue, Collations.KanjiKanaCollation).NotNullable();

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(RuleContent.GrammarRuleId, RuleContent.LanguageId);
	}

	private static void CreateRuleSearchByKanji(Migration migration)
	{
		const string tblName = Grammar.RuleSearchByKanji;

		migration.Create.Table(tblName)
			.WithColumn(RuleSearchByKanji.SearchText).AsString(Collations.KanjiKanaCollation).NotNullable()
			.WithForeignKeyToGrammarRule(RuleSearchByKanji.GrammarRuleId);

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(RuleSearchByKanji.SearchText, RuleSearchByKanji.GrammarRuleId);
	}

	private static void CreateRuleSearchByRomaji(Migration migration)
	{
		const string tblName = Grammar.RuleSearchByRomaji;

		migration.Create.Table(tblName)
			.WithColumn(RuleSearchByRomaji.SearchText).AsAnsiString().NotNullable()
			.WithForeignKeyToGrammarRule(RuleSearchByRomaji.GrammarRuleId);

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(RuleSearchByRomaji.SearchText, RuleSearchByRomaji.GrammarRuleId);
	}

	private static void CreateRuleSearchByLanguage(Migration migration)
	{
		const string tblName = Grammar.RuleSearchByLanguage;

		migration.Create.Table(tblName)
			.WithForeignKeyToLanguage(RuleSearchByLanguage.LanguageId)
			.WithColumn(RuleSearchByLanguage.SearchText).AsString().NotNullable()
			.WithForeignKeyToGrammarRule(RuleSearchByLanguage.GrammarRuleId);

		migration.Create.PrimaryKey()
			.OnTable(tblName)
			.Columns(RuleSearchByLanguage.LanguageId, RuleSearchByLanguage.SearchText, RuleSearchByLanguage.GrammarRuleId);
	}
}
