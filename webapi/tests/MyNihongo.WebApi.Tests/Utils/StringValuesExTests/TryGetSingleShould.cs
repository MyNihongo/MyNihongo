namespace MyNihongo.WebApi.Tests.Utils.StringValuesExTests;

public sealed class TryGetSingleShould
{
	[Fact]
	public void BeFalseIfNoValues()
	{
		var result = new StringValues()
			.TryGetSingle(out var outResult);

		result
			.Should()
			.BeFalse();

		outResult
			.Should()
			.BeEmpty();
	}

	[Fact]
	public void BeFalseIfMoreThanSingle()
	{
		var result = new StringValues(new[] { "value1", "value2" })
			.TryGetSingle(out var outResult);

		result
			.Should()
			.BeFalse();

		outResult
			.Should()
			.BeEmpty();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void BeFalseIfValueEmpty(string input)
	{
		var result = new StringValues(new[] { input })
			.TryGetSingle(out var outResult);

		result
			.Should()
			.BeFalse();

		outResult
			.Should()
			.BeEmpty();
	}

	[Fact]
	public void ReturnValue()
	{
		const string input = nameof(input);

		var result = new StringValues(new[] { input })
			.TryGetSingle(out var outResult);

		result
			.Should()
			.BeTrue();

		outResult
			.Should()
			.Be(input);
	}
}