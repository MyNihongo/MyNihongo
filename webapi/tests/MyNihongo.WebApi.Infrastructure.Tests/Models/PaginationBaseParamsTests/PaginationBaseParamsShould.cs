namespace MyNihongo.WebApi.Infrastructure.Tests.Models.PaginationBaseParamsTests;

public sealed class PaginationBaseParamsShould
{
	[Fact]
	public void SetNonNegativePageIndex()
	{
		const int fixture = -1,
			expected = 0;

		var result = new Record { PageIndex = fixture }.PageIndex;

		result
			.Should()
			.Be(expected);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public void KeepPageIndexIfNotNegative(int fixture)
	{
		var result = new Record { PageIndex = fixture }.PageIndex;

		result
			.Should()
			.Be(fixture);
	}

	[Theory]
	[InlineData(9)]
	[InlineData(36)]
	public void SetDefaultPageSizeIfOutsideBounds(int fixture)
	{
		const int expected = 35;

		var result = new Record { PageSize = fixture }.PageSize;

		result
			.Should()
			.Be(expected);
	}

	[Theory]
	[InlineData(10)]
	[InlineData(15)]
	[InlineData(35)]
	public void KeepPageSizeIfWithinBounds(int fixture)
	{
		var result = new Record { PageSize = fixture }.PageSize;

		result
			.Should()
			.Be(fixture);
	}

	private sealed record Record : PaginationBaseParams;
}