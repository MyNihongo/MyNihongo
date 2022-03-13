namespace MyNihongo.WebApi.Tests.Utils.StringExTests;

public sealed class HasSequenceShould
{
	[Fact]
	public void BeFalseIfInputLonger()
	{
		const string fixture = "123",
			input = "12345";

		var result = fixture.HasSequence(input);

		result
			.Should()
			.BeFalse();
	}

	[Fact]
	public void BeFalseIfNotStartsWith()
	{
		const string fixture = "12345",
			input = "12445";

		var result = fixture.HasSequence(input);

		result
			.Should()
			.BeFalse();
	}

	[Fact]
	public void BeTrueIfStartsWith()
	{
		const string fixture = "12345",
			input = "123";

		var result = fixture.HasSequence(input);

		result
			.Should()
			.BeTrue();
	}

	[Fact]
	public void BeTrueIfEquals()
	{
		const string fixture = "12345",
			input = "12345";

		var result = fixture.HasSequence(input);

		result
			.Should()
			.BeTrue();
	}
}