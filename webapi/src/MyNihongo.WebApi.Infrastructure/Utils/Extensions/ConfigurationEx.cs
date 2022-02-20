using System.Collections.Concurrent;

namespace MyNihongo.WebApi.Infrastructure;

internal static class ConfigurationEx
{
	private static readonly ConcurrentDictionary<ConnectionType, Lazy<string>> ConnectionStrings = new();

	public static string GetConnectionString(this IConfiguration configuration, ConnectionType type) =>
		ConnectionStrings.GetOrAdd(type, x =>
		{
			return new Lazy<string>(() => GetConnection(configuration, x));
		}).Value;

	private static string GetConnection(IConfiguration configuration, ConnectionType type)
	{
		configuration = configuration.GetRequiredSection("Database");

		var connectionStringBuilder = new SqlConnectionStringBuilder
		{
			DataSource = configuration["DataSource"],
			InitialCatalog = configuration["InitialCatalog"]
		};

		configuration = type switch
		{
			ConnectionType.Auth => configuration.GetRequiredSection("Auth"),
			_ => configuration.GetRequiredSection("Standard")
		};

		connectionStringBuilder.TrustServerCertificate = true;
		connectionStringBuilder.UserID = configuration["User"];
		connectionStringBuilder.Password = configuration["Password"];

#if RELEASE
		connectionStringBuilder.Password = "{required decryption}";
#endif

		return connectionStringBuilder.ConnectionString;
	}
}