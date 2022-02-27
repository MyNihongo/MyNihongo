namespace MyNihongo.Database.Abstractions.Tests.ProtoTypes.DoubleOptionalTests;

public sealed class FromDoubleShould
{
	[Fact]
	public void HaveValue()
	{
		const double fixture = 123d;

		var expected = new DoubleOptional
		{
			HasValue = true,
			Value = fixture
		};

		DoubleOptional result = fixture;

		result
			.Should()
			.Be(expected);
	}
}