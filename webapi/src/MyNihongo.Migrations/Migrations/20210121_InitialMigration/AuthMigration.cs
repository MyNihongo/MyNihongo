using MyNihongo.Migrations.Utils.Extensions;

namespace MyNihongo.Migrations.Migrations;

internal sealed class AuthMigration : IMigrationInternal
{
	public void Up(Migration migration)
	{
		var actions = new Action<Migration>[]
		{
			CreateAuthConnectionTable,
			CreateAuthConnectionTokenTable
		};

		migration.InvokeAll(actions);
	}

	public void Down(Migration migration)
	{
		var tables = new[]
		{
			Auth.ConnectionToken,
			Auth.Connection
		};

		migration.DropAll(tables);
	}

	private static void CreateAuthConnectionTable(Migration migration)
	{
		const string tblName = Auth.Connection;

		migration.Create.Table(tblName)
			.WithColumn(Connection.ConnectionId).AsGuid().PrimaryKey()
			.WithColumn(Connection.UserId).AsInt64().NotNullable().ForeignKey(Core.User, User.UserId)
			.WithColumn(Connection.IpAddress).AsAnsiString(16).NotNullable()
			.WithColumn(Connection.ClientInfo).AsString(255).NotNullable()
			.WithColumn(Connection.TicksLatestAccessed).AsInt64().NotNullable();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(Connection.UserId);
	}

	private static void CreateAuthConnectionTokenTable(Migration migration)
	{
		const string tblName = Auth.ConnectionToken;

		migration.Create.Table(tblName)
			.WithColumn(ConnectionToken.TokenId).AsGuid().PrimaryKey()
			.WithColumn(ConnectionToken.ConnectionId).AsGuid().NotNullable().ForeignKey(Auth.Connection, Connection.ConnectionId)
			.WithColumn(ConnectionToken.TicksValidTo).AsInt64().NotNullable();

		migration.Create.Index()
			.OnTable(tblName)
			.OnColumn(ConnectionToken.ConnectionId);
	}
}