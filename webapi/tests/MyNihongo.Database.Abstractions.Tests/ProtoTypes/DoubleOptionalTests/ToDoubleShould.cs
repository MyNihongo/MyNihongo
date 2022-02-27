namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.DoubleOptionalTests;

public sealed class ToDoubleShould
{
	[Theory]
	[InlineData(-1d)]
	[InlineData(0d)]
	[InlineData(1d)]
	[InlineData(-123.456d)]
	[InlineData(654.321d)]
	public void BeDoubleIfHasValue(double value)
	{
		double? result = new DoubleOptional
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
		double? result = new DoubleOptional
		{
			HasValue = false,
			Value = 123d
		};

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void BeNullIfNull()
	{
		DoubleOptional? fixture = null;
		double? result = fixture;

		result
			.Should()
			.BeNull();
	}
}