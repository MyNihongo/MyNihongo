using System.Collections.Concurrent;

namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.ConfigurationExTests;

public abstract class ConfigurationExTestsBase
{
	protected Mock<IConfiguration> MockConfiguration { get; } = new();

	protected IConfiguration CreateFixture() =>
		MockConfiguration.Object;

	internal static IReadOnlyDictionary<ConnectionType, string> GetConnections()
	{
		return typeof(ConfigurationEx)
			.GetStaticField<ConcurrentDictionary<ConnectionType, Lazy<string>>>("ConnectionStrings")
			.ToDictionary(x => x.Key, x => x.Value.Value);
	}
}