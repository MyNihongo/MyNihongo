// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji.KanjiGetListRequestHandlerTests;

public sealed class HandleShould : KanjiGetListRequestHandlerTestsBase
{
	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	public async Task InvokeByJlptIfSearchEmpty(string searchText)
	{
		var expected = new KanjiGetListByJlptParams();

		var req = new KanjiGetListRequest
		{
			SearchText = searchText
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		MockDatabase.Verify(x => x.QueryByJlptAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task NotInvokeIfSearchTextShort()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "a"
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task InvokeByLanguageAndRomaji()
	{
		const string searchText = nameof(searchText);

		var expected = new KanjiGetListByTextParams
		{
			Text = searchText,
			ByRomaji = true,
			ByLanguage = true
		};

		var req = new KanjiGetListRequest
		{
			SearchText = searchText
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		MockDatabase.Verify(x => x.QueryByTextAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Theory]
	[InlineData("ひ", "hi")]
	[InlineData("カ", "ka")]
	[InlineData("ひらがな", "hiragana")]
	[InlineData("カタカナ", "katakana")]
	[InlineData("ひタがナ", "hitagana")]
	public async Task InvokeByRomaji(string searchText, string romaji)
	{
		var expected = new KanjiGetListByTextParams
		{
			Text = romaji,
			ByRomaji = true,
			ByLanguage = false
		};

		var req = new KanjiGetListRequest
		{
			SearchText = searchText
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		MockDatabase.Verify(x => x.QueryByTextAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task TrimSearchText()
	{
		var searchText = string.Create(50, true, (span, _) =>
		{
			for (var i = 0; i < span.Length; i++)
				span[i] = 'a';
		});

		var expected = new KanjiGetListByTextParams
		{
			Text = searchText,
			ByRomaji = true,
			ByLanguage = true
		};

		var req = new KanjiGetListRequest
		{
			SearchText = searchText + '1'
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		MockDatabase.Verify(x => x.QueryByTextAsync(expected, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task NotInvokeIfTextUndefined()
	{
		var req = new KanjiGetListRequest
		{
			SearchText = "ひらがn"
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task InvokeByKanji()
	{
		var expected = new KanjiGetListByCharParams
		{
			Characters = new[] { '日', '本', '語', '好' }
		};

		var req = new KanjiGetListRequest
		{
			SearchText = "日本語が好き"
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		MockDatabase.Verify(x => x.QueryByCharAsync(ItIs.Equivalent(expected), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task TrimKanjiLength()
	{
		var list = new List<char>(26);

		for (int i = 0x5f00, j = 0; j < 25; i++, j++)
			list.Add((char)i);

		var expected = new KanjiGetListByCharParams
		{
			Characters = list.ToArray()
		};

		list.Add('娇');

		var req = new KanjiGetListRequest
		{
			SearchText = string.Join(null, list)
		};

		using var cts = new CancellationTokenSource();

		await CreateFixture()
			.HandleAsync(req, cts.Token);

		MockDatabase.Verify(x => x.QueryByCharAsync(ItIs.Equivalent(expected), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}