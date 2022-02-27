namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiGetListByJlptParamsTests;

public sealed class KanjiGetListByJlptParamsShould
{
	[Fact]
	public void SetNonNegativePageIndex()
	{
		const int fixture = -1,
			expected = 0;

		var result = new KanjiGetListByJlptParams { PageIndex = fixture }.PageIndex;

		result
			.Should()
			.Be(expected);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public void KeepPageIndexIfNotNegative(int fixture)
	{
		var result = new KanjiGetListByJlptParams { PageIndex = fixture }.PageIndex;

		result
			.Should()
			.Be(fixture);
	}

	[Fact]
	public void HaveZeroIndexByDefault()
	{
		const int expected = 0;

		var result = new KanjiGetListByJlptParams()
			.PageIndex;

		result
			.Should()
			.Be(expected);
	}

	[Theory]
	[InlineData(9)]
	[InlineData(36)]
	public void SetDefaultPageSizeIfOutsideBounds(int fixture)
	{
		const int expected = 35;

		var result = new KanjiGetListByJlptParams { PageSize = fixture }.PageSize;

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
		var result = new KanjiGetListByJlptParams { PageSize = fixture }.PageSize;

		result
			.Should()
			.Be(fixture);
	}

	[Fact]
	public void HavePageSizeByDefault()
	{
		const int expected = 35;

		var result = new KanjiGetListByJlptParams()
			.PageSize;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void SetJlptToNullIfDefault()
	{
		var result = new KanjiGetListByJlptParams { JlptLevel = JlptLevel.UndefinedJlptLevel }
			.JlptLevel;

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void SetJlpt()
	{
		const JlptLevel expected = JlptLevel.N2;

		var result = new KanjiGetListByJlptParams { JlptLevel = expected }
			.JlptLevel;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HaveNullJlptByDefault()
	{
		var result = new KanjiGetListByJlptParams()
			.JlptLevel;

		result
			.Should()
			.BeNull();
	}

	[Fact]
	public void SetLanguageToEnglishIfDefault()
	{
		const Language expected = Language.English;

		var result = new KanjiGetListByJlptParams { Language = Language.UndefinedLanguage }
			.Language;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void SetLanguage()
	{
		const Language expected = Language.Russian;

		var result = new KanjiGetListByJlptParams { Language = Language.Russian }
			.Language;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HaveEnglishByDefault()
	{
		const Language expected = Language.English;

		var result = new KanjiGetListByJlptParams()
			.Language;

		result
			.Should()
			.Be(expected);
	}
}