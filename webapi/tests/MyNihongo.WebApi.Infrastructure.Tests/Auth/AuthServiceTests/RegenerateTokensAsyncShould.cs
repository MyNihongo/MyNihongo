// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Auth.AuthServiceTests;

public sealed class RegenerateTokensAsyncShould : AuthServiceTestsBase
{
	[Fact]
	public async Task ReturnTokenIds()
	{
		Guid accessTokenId = Guid.NewGuid(),
			refreshTokenId = Guid.NewGuid();

		var expected = new AuthRegenerateTokensResult(accessTokenId, refreshTokenId);
		var parameters = new AuthRecreateTokensParams
		{
			ConnectionId = Guid.NewGuid(),
			TicksRefreshValidTo = 123L,
			TicksAccessValidTo = 321L
		};

		using var cts = new CancellationTokenSource();

		MockDatabase
			.Setup(x => x.RecreateTokensAsync(parameters, cts.Token))
			.ReturnsAsync(new AuthRecreateTokensResult { AccessTokenId = accessTokenId, RefreshTokenId = refreshTokenId });

		var result = await CreateFixture()
			.RegenerateTokensAsync(parameters, cts.Token);

		result
			.Should()
			.Be(expected);

		MockDatabase.Verify(x => x.RecreateTokensAsync(parameters, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}