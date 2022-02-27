namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.StringOptionalTests;

public sealed class ToStringShould
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	[InlineData("value")]
	public void BeStringIfHasValue(string value)
	{
		string? result = new StringOptional
		{
			HasValue = true,
			Value = value
		};

		result
			.Should()
			.Be(value);
	}

	[Fact]
	public void BeNullIfNotHasValue()
	{
		string? result = new StringOptional
		{
			HasValue = false,
			Value = "value"
		};

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void BeNullIfNull()
	{
		StringOptional? fixture = null;
		string? result = fixture;

		result
			.Should()
			.BeNull();
	}
}