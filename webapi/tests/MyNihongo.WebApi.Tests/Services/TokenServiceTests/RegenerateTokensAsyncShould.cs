// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Tests.Services.TokenServiceTests;

public sealed class RegenerateTokensAsyncShould : TokenServiceTestsBase
{
	[Fact]
	public async Task ReturnNewTokens()
	{
		const long userId = 1L;
		Guid connectionId = Guid.Parse("04f0e5bd-479d-4548-acf2-910e17d4cd1a"),
			accessTokenId = Guid.Parse("5665ab5e-bc41-40ae-9a30-f09b98e0cb0b"),
			refreshTokenId = Guid.Parse("7d775f17-aafa-42a4-a842-2a23d140fed6");

		JwtClaims accessExpected = new(accessTokenId, userId),
			refreshExpected = new(refreshTokenId, userId);

		var input = new AuthConnectionValidationResult
		{
			ConnectionId = connectionId,
			UserId = userId
		};

		using var cts = new CancellationTokenSource();

		MockDateTimeService.SetupInstant(2022, 3, 6, 12);
		MockDateTimeService.SetupUtcNow();

		MockAuthService
			.Setup(x => x.RegenerateTokensAsync(It.Is<AuthRecreateTokensParams>(y => y.ConnectionId == input.ConnectionId), cts.Token))
			.ReturnsAsync(new AuthRegenerateTokensResult(accessTokenId, refreshTokenId));

		var (accessToken, refreshToken) = await CreateFixture()
			.RegenerateTokensAsync(input, cts.Token);

		VerifyAccessToken(accessToken, accessExpected);
		VerifyRefreshToken(refreshToken, refreshExpected);
	}
}