namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.ConfigurationExTests;

public sealed class GetConnectionStringShould : ConfigurationExTestsBase
{
	[Fact]
	public void CreateAndSaveConnectionString()
	{
		const ConnectionType connectionType = ConnectionType.Standard;
		const string expected = "Data Source=DataSource;Initial Catalog=InitialCatalog;User ID=User;Password=Password;Trust Server Certificate=True";

		var expectedConnections = new Dictionary<ConnectionType, string>
		{
			{ connectionType, expected }
		};

		MockConfiguration
			.SetupConfiguration()
			.Returns(new
			{
				Database = new
				{
					DataSource = "DataSource",
					InitialCatalog = "InitialCatalog",
					Standard = new
					{
						User = "User",
						Password = "Password"
					}
				}
			});

		var result = CreateFixture()
			.GetConnectionString(connectionType);

		result
			.Should()
			.Be(expected);

		GetConnections()
			.Should()
			.BeEquivalentTo(expectedConnections);
	}
}