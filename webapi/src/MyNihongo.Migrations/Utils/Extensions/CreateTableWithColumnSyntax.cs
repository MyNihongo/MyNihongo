using FluentMigrator.Builders.Create.Table;

namespace MyNihongo.Migrations.Utils.Extensions;

internal static class CreateTableWithColumnSyntax
{
	public static ICreateTableWithColumnSyntax WithForeignKeyToLanguage(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsByte().NotNullable().ForeignKey(Core.Lang, Lang.LanguageId);
	}

	public static ICreateTableWithColumnSyntax WithForeignKeyToGrammarRule(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsInt16().ForeignKey(Grammar.RuleMasterData, RuleMasterData.GrammarRuleId).OnDelete(Rule.Cascade);
	}
}
