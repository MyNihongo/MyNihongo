namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.Int32OptionalTests;

public sealed class FromIntShould
{
	[Fact]
	public void HaveValue()
	{
		const int fixture = 123;

		var expected = new Int32Optional
		{
			HasValue = true,
			Value = fixture
		};

		Int32Optional result = fixture;

		result
			.Should()
			.Be(expected);
	}
}