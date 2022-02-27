namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.Int32OptionalTests;

public sealed class ToIntShould
{
	[Theory]
	[InlineData(-1)]
	[InlineData(0)]
	[InlineData(1)]
	public void BeIntIfHasValue(int value)
	{
		int? result = new Int32Optional
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
		int? result = new Int32Optional
		{
			HasValue = false,
			Value = 123
		};

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void BeNullIfNull()
	{
		Int32Optional? fixture = null;
		int? result = fixture;

		result
			.Should()
			.BeNull();
	}
}