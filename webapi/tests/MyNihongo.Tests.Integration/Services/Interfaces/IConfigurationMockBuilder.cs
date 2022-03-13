namespace MyNihongo.Tests.Integration.Services;

public interface IConfigurationMockBuilder
{
	void AppendRootSection(string name, object data);
}