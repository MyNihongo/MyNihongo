using FluentMigrator.Builders.Create.Table;

namespace MyNihongo.Migrations.Utils.Extensions;

internal static class CreateTableWithColumnSyntax
{
	public static ICreateTableWithColumnSyntax WithForeignKeyToLanguage(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsByte().NotNullable().ForeignKey(Core.Lang, Lang.LanguageId);
	}

	public static ICreateTableWithColumnSyntax WithForeignKeyToUser(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsInt64().NotNullable().ForeignKey(Core.User, User.UserId).OnDelete(Rule.Cascade);
	}

	public static ICreateTableWithColumnSyntax WithForeignKeyToConnection(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsGuid().NotNullable().ForeignKey(Auth.Connection, Connection.ConnectionId).OnDelete(Rule.Cascade);
	}

	public static ICreateTableWithColumnSyntax WithForeignKeyToGrammarRule(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsInt16().ForeignKey(Grammar.RuleMasterData, RuleMasterData.GrammarRuleId).OnDelete(Rule.Cascade);
	}

	public static ICreateTableWithColumnSyntax WithForeignKeyToKanji(this ICreateTableWithColumnSyntax @this, string name)
	{
		return @this.WithColumn(name).AsInt16().NotNullable().ForeignKey(Kanji.MasterData, KanjiMasterData.KanjiId).OnDelete(Rule.Cascade);
	}
}
