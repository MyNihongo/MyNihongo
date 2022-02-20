using MediatR;

namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.StringExTests;

public sealed class TrimExShould
{
	[Fact]
	public void BeEmptyIfNull()
	{
		string? fixture = null;

		var result = fixture.TrimEx();

		result
			.Should()
			.BeEmpty();
	}

	[Fact]
	public void TrimText()
	{
		const string fixture = "  abc   ",
			expected = "abc";

		var result = fixture.TrimEx();

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void TrimTextIfLengthTooLarge()
	{
		var expected = string.Create(50, Unit.Value, (span, _) =>
		{
			for (var i = 0; i < span.Length; i++)
				span[i] = (i % 10).ToString().Single();
		});

		var fixture = $"  {expected}_some_more_text ";

		var result = fixture.TrimEx();

		result
			.Should()
			.Be(expected);
	}
}