using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Dmf;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using MyNihongo.Migrations.Models.Options;
using MyNihongo.Migrations.Resources;

namespace MyNihongo.Migrations.Services;

internal sealed class ScriptRunner : IScriptRunner
{
	private readonly IOptions<ConnectionString> _connectionString;

	public ScriptRunner(IOptions<ConnectionString> connectionString)
	{
		_connectionString = connectionString;
	}

	public void Execute()
	{
		using var connection = new SqlConnection(_connectionString.Value.Value);
		connection.Open();

		EnsureLogin(connection);
		RecreateStoredProcedures(connection);
	}

	private static void EnsureLogin(SqlConnection connection)
	{
		var users = new[]
		{
			DatabaseConst.StandardUser,
			DatabaseConst.AuthUser
		};

		using var command = new SqlCommand { Connection = connection };

		for (var i = 0; i < users.Length; i++)
		{
			var texts = new[]
			{
// Ensure the server login
$@"IF NOT EXISTS (
	SELECT TOP 1
		NULL
	FROM
		master.sys.server_principals
	WHERE
		name = '{users[i]}'
) BEGIN
	CREATE LOGIN [{users[i]}] WITH PASSWORD = N'{DatabaseConst.DefaultPassword}'
END",

// Ensure the DB login
$@"IF NOT EXISTS (
	SELECT TOP 1
		NULL
	FROM
		sys.database_principals
	WHERE
		name = '{users[i]}'
) BEGIN
	CREATE USER [{users[i]}] FOR LOGIN [{users[i]}] 
END"
			};

			for (var j = 0; j < texts.Length; j++)
			{
				command.CommandText = texts[j];
				command.ExecuteNonQuery();
			}
		}
	}

	private static void RecreateStoredProcedures(SqlConnection connection)
	{
		ServerConnection? serverConnection = null;
		try
		{
			serverConnection = new ServerConnection(connection);
			var server = new Server(serverConnection);

			foreach (var scriptContent in GetScriptFiles())
			{
				server.ConnectionContext.ExecuteNonQuery(scriptContent);
				SetExecutePermission(server, scriptContent);
			}
		}
		finally
		{
			serverConnection?.Disconnect();
		}
	}

	private static void SetExecutePermission(IAlienRoot server, string scriptContent)
	{
		const string startText = "CREATE PROCEDURE ";
		var startIndex = scriptContent.IndexOf(startText, StringComparison.OrdinalIgnoreCase);
		if (startIndex == -1)
			return;

		var endIndex = scriptContent.IndexOf('\r', startIndex);
		var spName = scriptContent[(startIndex + startText.Length)..endIndex];
		var prefix = GetSpNamePrefix(spName);

		string allowUSer, denyUser;
		if (prefix == "Auth")
		{
			allowUSer = DatabaseConst.AuthUser;
			denyUser = DatabaseConst.StandardUser;
		}
		else
		{
			allowUSer = DatabaseConst.StandardUser;
			denyUser = DatabaseConst.AuthUser;
		}

		var text =
$@"GRANT EXECUTE ON OBJECT::{spName} TO [{allowUSer}]
GO

DENY EXECUTE ON OBJECT::{spName} TO [{denyUser}]
GO";

		server.ConnectionContext.ExecuteNonQuery(text);
	}

	private static IEnumerable<string> GetScriptFiles()
	{
		var basePath = GetBasePath();
		foreach (var file in Directory.EnumerateFiles(basePath, "*.sql", SearchOption.AllDirectories))
		{
			using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var reader = new StreamReader(fileStream);

			yield return reader.ReadToEnd();
		}
	}

	private static string GetBasePath()
	{
		var path = AppContext.BaseDirectory;

		do
		{
			path = Path.GetDirectoryName(path);
		} while (path != null && !path.EndsWith("webapi"));

		if (string.IsNullOrEmpty(path))
			throw new DirectoryNotFoundException("Cannot find the project root directory");

		return Path.Combine(path, "sql");
	}

	private static string GetSpNamePrefix(string spName)
	{
		if (string.IsNullOrEmpty(spName))
			throw new ArgumentNullException(nameof(spName));

		for (int i = 0, start = -1; i < spName.Length; i++)
		{
			if (!char.IsUpper(spName[i]))
				continue;

			if (start != -1)
				return spName[start..i];

			start = i;
		}

		throw new InvalidOperandException("Cannot find the prefix");
	}
}