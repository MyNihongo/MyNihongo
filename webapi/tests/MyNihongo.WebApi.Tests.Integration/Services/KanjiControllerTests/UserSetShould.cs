using MyNihongo.WebApi.Infrastructure.Kanji;

namespace MyNihongo.WebApi.Tests.Integration.Services.KanjiControllerTests;

[Collection(WebApiCollection.Name), UsesVerify]
public sealed class UserSetShould : KanjiControllerTestsBase
{
	public UserSetShould(WebApiSnapshotFixture snapshot)
		: base(snapshot)
	{
	}

	[Fact, Order(-1)]
	public async Task UpdateConnection()
	{
		var req = new KanjiUserSetRequest
		{
			KanjiId = 2,
			FavouriteRating = 2.5d
		};

		await CreateFixture()
			.UserSetAsync(req, new Metadata
			{
				{ ApiConst.Headers.ClientInfo, "Updated client info" },
				{ "Cookie", $"{ApiConst.Cookies.AccessToken}={Snapshot.User1AccessToken}" },
				{ "Cookie", $"{ApiConst.Cookies.RefreshToken}={Snapshot.User1RefreshToken}" }
			});

		var result = await GetConnectionDataAsync(1L)
			.GetJsonAsync();

		await Verify(result);
	}

	[Fact]
	public async Task NoClientInfo()
	{
		var req = new KanjiUserSetRequest
		{
			KanjiId = 1
		};

		var func = async () => await CreateFixture()
			.UserSetAsync(req);

		await func
			.Should()
			.ThrowUnauthorisedAsync();
	}

	[Fact]
	public async Task NoTokens()
	{
		var req = new KanjiUserSetRequest
		{
			KanjiId = 1
		};

		var func = async () => await CreateFixture()
			.UserSetAsync(req, new Metadata
			{
				{ ApiConst.Headers.ClientInfo, "Microsoft Edge 98" }
			});

		await func
			.Should()
			.ThrowUnauthorisedAsync();
	}

	[Fact]
	public async Task InvalidRefresh()
	{
		const string invalidRefreshToken =
			"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
			"eyJhcHoiOiI3ZDc3NWYxNy1hYWZhLTQyYTQtYTg0Mi0yYTIzZDE0MGZlZDYiLCJzbGIiOiIxIiwibmJmIjoxNjQ2NTg1NTM5LCJleHAiOjE2NDY1ODU4MzksImlhdCI6MTY0NjU4NTUzOSwiaXNzIjoiVmFsaWRJc3N1ZXIifQ." +
			"3WXikHPvJtnSxNwGG0SSScxNCzuux7oaP7dAncQ3LBw";

		var req = new KanjiUserSetRequest
		{
			KanjiId = 1
		};

		var func = async () => await CreateFixture()
			.UserSetAsync(req, new Metadata
			{
				{ ApiConst.Headers.ClientInfo, "Microsoft Edge 98" },
				{ "Cookie", $"{ApiConst.Cookies.RefreshToken}={invalidRefreshToken}" }
			});

		await func
			.Should()
			.ThrowUnauthorisedAsync();
	}

	[Fact]
	public async Task InvalidAccess()
	{
		const string invalidAccessToken =
			"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
			"eyJhcHoiOiJiN2Y2ZDg1Ni0yZGRmLTQ4NTktYjViZS1kODZkNDNhNDk3YzciLCJzbGIiOiIxIiwibmJmIjoxNjQ3MTA1MjkzLCJleHAiOjE2NDcxMDYxOTMsImlhdCI6MTY0NzEwNTI5MywiaXNzIjoiVmFsaWRJc3N1ZXIifQ." +
			"BVLoCqBMHuuRXZwL-hYrUtD-PMtTxXi3GRpIKb_yCEE";

		var req = new KanjiUserSetRequest
		{
			KanjiId = 1
		};

		var func = async () => await CreateFixture()
			.UserSetAsync(req, new Metadata
			{
				{ ApiConst.Headers.ClientInfo, "Microsoft Edge 98" },
				{ "Cookie", $"{ApiConst.Cookies.AccessToken}={invalidAccessToken}" },
				{ "Cookie", $"{ApiConst.Cookies.RefreshToken}={Snapshot.User1RefreshToken}" }
			});

		await func
			.Should()
			.ThrowUnauthorisedAsync();
	}

	[Fact, Order(1)]
	public async Task ValidAccess()
	{
		var req = new KanjiUserSetRequest
		{
			KanjiId = 1,
			Notes = "more notes"
		};

		var result = await CreateFixture()
			.UserSetAsync(req, new Metadata
			{
				{ ApiConst.Headers.ClientInfo, "Microsoft Edge 98" },
				{ "Cookie", $"{ApiConst.Cookies.AccessToken}={Snapshot.User1AccessToken}" },
				{ "Cookie", $"{ApiConst.Cookies.RefreshToken}={Snapshot.User1RefreshToken}" }
			}).GetJsonAsync();

		await Verify(result);
	}

	[Fact, Order(2)]
	public async Task ValidRefresh()
	{
		var req = new KanjiUserSetRequest
		{
			KanjiId = 1,
			FavouriteRating = 2.5d
		};

		var result = await CreateFixture()
			.UserSetAsync(req, new Metadata
			{
				{ ApiConst.Headers.ClientInfo, "Microsoft Edge 98" },
				{ "Cookie", $"{ApiConst.Cookies.RefreshToken}={Snapshot.User1RefreshToken}" }
			}).GetJsonAsync();

		await Verify(result);
		await VerifyTokensRegeneratedAsync(1L);
	}
}