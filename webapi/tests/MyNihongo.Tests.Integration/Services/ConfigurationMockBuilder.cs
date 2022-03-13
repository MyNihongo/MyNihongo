using System.Dynamic;

namespace MyNihongo.Tests.Integration.Services;

internal sealed class ConfigurationMockBuilder : IConfigurationMockBuilder
{
	private readonly Mock<IConfiguration> _mockConfiguration;
	private readonly Dictionary<string, object> _sections = new();

	public ConfigurationMockBuilder(Mock<IConfiguration> mockConfiguration)
	{
		_mockConfiguration = mockConfiguration;
	}

	public void AppendRootSection(string name, object data) =>
		_sections.Add(name, data);

	internal void Setup()
	{
		var expandoObj = new ExpandoObject();
		var dictionary = (IDictionary<string, object>)expandoObj!;

		foreach (var (key, value) in _sections)
			dictionary.Add(key, value);

		_mockConfiguration
			.SetupConfiguration()
			.Returns(expandoObj);
	}
}