using DiffEngine;
using Microsoft.Extensions.DependencyInjection;
using MyNihongo.Migrations.Resources;
using MyNihongo.Migrations.Services;
using MyNihongo.Migrations.Utils;
using MyNihongo.Tests.Integration.Models.Core;
using MyNihongo.Tests.Integration.Services;
using MyNihongo.Tests.Integration.Utils;
using ThrowawayDb;

namespace MyNihongo.Tests.Integration;

public abstract class DatabaseFixture : IDisposable
{
	private readonly ThrowawayDatabase _database;
	private readonly Mock<IConfiguration> _mockConfiguration = new();

	static DatabaseFixture()
	{
		DiffTools.UseOrder(DiffTool.VisualStudio, DiffTool.BeyondCompare);
	}

	protected DatabaseFixture(Action<DatabaseConnection, IConfiguration> prepareData, Action<IConfigurationMockBuilder>? prepareConfiguration = null)
	{
		var localInstance = SqlServerUtils.GetLocalInstance();
		var connectionStringBuilder = new SqlConnectionStringBuilder
		{
			DataSource = localInstance,
			IntegratedSecurity = true,
			TrustServerCertificate = true
		};

		try
		{
			_database = ThrowawayDatabase.Create(connectionStringBuilder.ConnectionString, "_mynihongo_");

			connectionStringBuilder.InitialCatalog = _database.Name;
			RunMigrations(connectionStringBuilder.ConnectionString);

			var configurationBuilder = new ConfigurationMockBuilder(_mockConfiguration);
			prepareConfiguration?.Invoke(configurationBuilder);
			PrepareBaseConfiguration(configurationBuilder, localInstance);

			using (var connection = OpenConnection())
			{
				PrepareBaseData(connection);
				prepareData(connection, _mockConfiguration.Object);
			}

			_database.CreateSnapshot();
		}
		catch
		{
			_database?.Dispose();
			throw;
		}
	}

	protected internal IConfiguration Configuration => _mockConfiguration.Object;

	internal DatabaseConnection OpenConnection() =>
		new(_database.ConnectionString);

	public virtual void Dispose()
	{
		_database.Dispose();
		GC.SuppressFinalize(this);
	}

	internal void RestoreSnapshot() =>
		_database.RestoreSnapshot();

	private static void RunMigrations(string connectionString)
	{
		using var serviceProvider = new ServiceCollection()
			.AddMigrator(connectionString)
			.BuildServiceProvider(true);

		using var scope = serviceProvider.CreateScope();

		scope.ServiceProvider.GetRequiredService<IMigrationService>()
			.Migrate();
	}

	private void PrepareBaseConfiguration(ConfigurationMockBuilder builder, string localInstance)
	{
		builder.AppendRootSection("Database", new
		{
			DataSource = localInstance,
			InitialCatalog = _database.Name,
			Standard = new
			{
				User = DatabaseConst.StandardUser,
				Password = DatabaseConst.DefaultPassword
			},
			Auth = new
			{
				User = DatabaseConst.AuthUser,
				Password = DatabaseConst.DefaultPassword
			}
		});

		builder.Setup();
	}

	private static void PrepareBaseData(DatabaseConnection connection)
	{
		connection.Users.BulkCopy(new CoreUserDatabaseRecord[]
		{
			new()
			{
				UserId = 1L,
				EmailHash = new byte[] { 1, 2 },
				PasswordHash = new byte[] { 3, 4 }
			},
			new()
			{
				UserId = 2L,
				EmailHash = new byte[] { 5, 6 },
				PasswordHash = new byte[] { 7, 8 }
			}
		});
	}
}