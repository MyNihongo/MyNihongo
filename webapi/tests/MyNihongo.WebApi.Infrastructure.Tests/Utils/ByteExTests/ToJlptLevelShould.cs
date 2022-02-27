namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.ByteExTests;

public sealed class ToJlptLevelShould
{
	[Fact]
	public void BeDefaultIfNull()
	{
		byte? fixture = null;
		const JlptLevel expected = JlptLevel.UndefinedJlptLevel;

		var result = fixture.ToJlptLevel();

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void BeValueIfNotNull()
	{
		byte? fixture = 3;
		const JlptLevel expected = JlptLevel.N3;

		var result = fixture.ToJlptLevel();

		result
			.Should()
			.Be(expected);
	}
}