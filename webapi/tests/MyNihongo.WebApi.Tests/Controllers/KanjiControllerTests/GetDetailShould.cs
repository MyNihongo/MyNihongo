// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Tests.Controllers.KanjiControllerTests;

public sealed class GetDetailShould : KanjiControllerTestsBase
{
	[Fact]
	public async Task InvokeMediator()
	{
		const long userId = 123L;
		MockUserSession.SetupUserId(userId);

		var expected = new KanjiGetDetailRequest
		{
			UserId = userId
		};

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		await CreateFixture()
			.GetDetail(new KanjiGetDetailRequest(), ctx);

		MockMediator.Verify(x => x.Send(ItIs.JsonEquivalent(expected), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task ReturnResult()
	{
		var expected = new KanjiGetDetailResponse
		{
			Character = "abc",
			JlptLevel = JlptLevel.N3
		};

		MockMediator.SetupSend(expected);

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		var req = new KanjiGetDetailRequest();

		var result = await CreateFixture()
			.GetDetail(req, ctx);

		result
			.Should()
			.Be(expected);
	}
}