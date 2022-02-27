namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.Int32OptionalTests;

public sealed class ToByteShould
{
	[Theory]
	[InlineData(byte.MinValue)]
	[InlineData(byte.MaxValue)]
	public void BeByteIfHasValue(byte value)
	{
		byte? result = new Int32Optional
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
		byte? result = new Int32Optional
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
		byte? result = fixture;

		result
			.Should()
			.BeNull();
	}

	[Theory]
	[InlineData(-1)]
	[InlineData(256)]
	public void BeNullIfOutsideBounds(int value)
	{
		byte? result = new Int32Optional
		{
			HasValue = true,
			Value = value
		};

		result
			.Should()
			.BeNull();
	}
}