// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Tests.Controllers.KanjiControllerTests;

public sealed class UserAddShould : KanjiControllerTestsBase
{
	[Fact]
	public async Task InvokeMediator()
	{
		const int kanjiId = 1;
		const long userId = 123L;
		MockUserSession.SetupRequiredUserId(userId);

		var expected = new KanjiUserAddRequest
		{
			KanjiId = kanjiId,
			UserId = userId
		};

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		await CreateFixture()
			.UserAdd(new KanjiUserAddRequest { KanjiId = kanjiId }, ctx);

		MockMediator.Verify(x => x.Send(ItIs.JsonEquivalent(expected), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task ReturnResult()
	{
		var expected = new KanjiUserAddResponse
		{
			FavouriteRating = 2.5d,
			Notes = "notes"
		};

		MockMediator.SetupSend(expected);

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		var req = new KanjiUserAddRequest();

		var result = await CreateFixture()
			.UserAdd(req, ctx);

		result
			.Should()
			.Be(expected);
	}
}