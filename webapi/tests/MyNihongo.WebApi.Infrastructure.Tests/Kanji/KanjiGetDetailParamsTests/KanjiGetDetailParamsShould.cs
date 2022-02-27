namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiGetDetailParamsTests;

public sealed class KanjiGetDetailParamsShould
{
	[Fact]
	public void SetLanguageToEnglishIfDefault()
	{
		const Language expected = Language.English;

		var result = new KanjiGetDetailParams { Language = Language.UndefinedLanguage }
			.Language;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void SetLanguage()
	{
		const Language expected = Language.Russian;

		var result = new KanjiGetDetailParams { Language = Language.Russian }
			.Language;

		result
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HaveEnglishByDefault()
	{
		const Language expected = Language.English;

		var result = new KanjiGetDetailParams()
			.Language;

		result
			.Should()
			.Be(expected);
	}
}