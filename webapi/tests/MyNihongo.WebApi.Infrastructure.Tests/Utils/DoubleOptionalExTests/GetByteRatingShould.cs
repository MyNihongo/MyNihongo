namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.DoubleOptionalExTests;

public sealed class GetByteRatingShould
{
	[Fact]
	public void BeNullIfNoValue()
	{
		var result = new DoubleOptional
		{
			HasValue = false,
			Value = 123d
		}.GetByteRating();

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void BeNullIfNull()
	{
		DoubleOptional? fixture = null;
		var result = fixture.GetByteRating();

		result
			.Should()
			.BeNull();
	}

	[Theory]
	[InlineData(-0.1d)]
	[InlineData(5.01d)]
	public void BeNullIfOutsideBounds(double value)
	{
		var result = new DoubleOptional
		{
			HasValue = true,
			Value = value
		}.GetByteRating();

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void ConvertWithoutDecimalDigit()
	{
		const double value = 3d;
		const byte expected = 30;

		var result = new DoubleOptional
		{
			HasValue = true,
			Value = value
		}.GetByteRating();

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ConvertWithOneDecimalDigit()
	{
		const double value = 3.4d;
		const byte expected = 34;

		var result = new DoubleOptional
		{
			HasValue = true,
			Value = value
		}.GetByteRating();

		result
			.Should()
			.Be(expected);
	}

	[Theory]
	[InlineData(3.44d)]
	[InlineData(3.444d)]
	public void ConvertWithTwoOrMoreDecimalDigits(double value)
	{
		const byte expected = 34;

		var result = new DoubleOptional
		{
			HasValue = true,
			Value = value
		}.GetByteRating();

		result
			.Should()
			.Be(expected);
	}
}