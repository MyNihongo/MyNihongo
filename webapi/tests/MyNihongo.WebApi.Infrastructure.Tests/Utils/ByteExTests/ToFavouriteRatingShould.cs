namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.ByteExTests;

public sealed class ToFavouriteRatingShould
{
	[Fact]
	public void ConvertToDouble()
	{
		const byte fixture = 35;
		const double expected = 3.5d;

		var result = fixture.ToFavouriteRating();

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void BeZero()
	{
		const byte fixture = 0;
		const double expected = 0d;

		var result = fixture.ToFavouriteRating();

		result
			.Should()
			.Be(expected);
	}
}