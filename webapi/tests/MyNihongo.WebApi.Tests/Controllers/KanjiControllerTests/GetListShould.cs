// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Tests.Controllers.KanjiControllerTests;

public sealed class GetListShould : KanjiControllerTestsBase
{
	private readonly Mock<IServerStreamWriter<KanjiGetListResponse>> _mockResponse = new();

	[Fact]
	public async Task InvokeMediator()
	{
		const long userId = 123L;
		MockUserSession.SetupUserId(userId);

		var expected = new KanjiGetListRequest
		{
			UserId = userId
		};

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		await CreateFixture()
			.GetList(new KanjiGetListRequest(), _mockResponse.Object, ctx);

		MockMediator.Verify(x => x.CreateStream(ItIs.JsonEquivalent(expected), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task WriteItems()
	{
		var expected = new KanjiGetListResponse[]
		{
			new() { Character = "abc" },
			new() { Character = "cde" }
		};

		MockMediator.SetupCreateStream(expected);
		MockUserSession.SetupUserId(null);

		var req = new KanjiGetListRequest();

		using var cts = new CancellationTokenSource();
		var ctx = TestServerCallContext.Create(ct: cts.Token);

		await CreateFixture()
			.GetList(req, _mockResponse.Object, ctx);

		_mockResponse.VerifyWrite(expected);
		MockMediator.Verify(x => x.CreateStream(ItIs.JsonEquivalent(req), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	protected override void VerifyNoOtherCalls()
	{
		_mockResponse.VerifyNoOtherCalls();
		base.VerifyNoOtherCalls();
	}
}