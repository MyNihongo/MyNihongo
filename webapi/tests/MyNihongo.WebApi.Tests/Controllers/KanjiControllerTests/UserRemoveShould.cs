// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Tests.Controllers.KanjiControllerTests;

public sealed class UserRemoveShould : KanjiControllerTestsBase
{
	[Fact]
	public async Task InvokeMediator()
	{
		const int kanjiId = 1;
		const long userId = 123L;
		MockUserSession.SetupRequiredUserId(userId);

		var expected = new KanjiUserRemoveRequest
		{
			KanjiId = kanjiId,
			UserId = userId
		};

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		await CreateFixture()
			.UserRemove(new KanjiUserRemoveRequest { KanjiId = kanjiId }, ctx);

		MockMediator.Verify(x => x.Send(ItIs.JsonEquivalent(expected), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task ReturnResult()
	{
		var expected = new KanjiUserRemoveResponse();
		MockMediator.SetupSend(expected);

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		var req = new KanjiUserRemoveRequest();

		var result = await CreateFixture()
			.UserRemove(req, ctx);

		result
			.Should()
			.Be(expected);
	}
}