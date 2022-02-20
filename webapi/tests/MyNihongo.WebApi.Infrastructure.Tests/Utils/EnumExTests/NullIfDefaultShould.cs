namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.EnumExTests;

public sealed class NullIfDefaultShould
{
	[Fact]
	public void ReturnNullIfDefault()
	{
		const JlptLevel fixture = JlptLevel.UndefinedJlptLevel;

		var result = fixture.NullIfDefault();

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void ReturnValueIfNotDefault()
	{
		var fixtures = Enum.GetValues<JlptLevel>()
			.Where(x => x != JlptLevel.UndefinedJlptLevel);

		foreach (var fixture in fixtures)
		{
			var result = fixture.NullIfDefault();

			result
				.Should()
				.Be(fixture);
		}
	}
}