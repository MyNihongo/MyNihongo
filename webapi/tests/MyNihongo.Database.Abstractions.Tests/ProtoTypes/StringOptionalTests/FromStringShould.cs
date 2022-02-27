namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.StringOptionalTests;

public sealed class FromStringShould
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("value")]
	public void HaveValueIfNotNull(string fixture)
	{
		var expected = new StringOptional
		{
			HasValue = true,
			Value = fixture
		};

		StringOptional result = fixture;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void NotHaveValueIfNullString()
	{
		var expected = new StringOptional
		{
			HasValue = false
		};

		string? fixture = null;
		StringOptional result = fixture;

		result
			.Should()
			.Be(expected);
	}
}