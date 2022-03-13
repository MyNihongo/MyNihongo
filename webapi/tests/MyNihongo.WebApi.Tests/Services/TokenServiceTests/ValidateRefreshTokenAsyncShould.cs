// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Tests.Services.TokenServiceTests;

public sealed class ValidateRefreshTokenAsyncShould : TokenServiceTestsBase
{
	[Fact]
	public async Task BeInvalidIfExpired()
	{
		const string token =
			"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
			"eyJhcHoiOiI3ZDc3NWYxNy1hYWZhLTQyYTQtYTg0Mi0yYTIzZDE0MGZlZDYiLCJzbGIiOiIxIiwibmJmIjoxNjQ2NTg1NTM5LCJleHAiOjE2NDY1ODU4MzksImlhdCI6MTY0NjU4NTUzOSwiaXNzIjoiVmFsaWRJc3N1ZXIifQ." +
			"3WXikHPvJtnSxNwGG0SSScxNCzuux7oaP7dAncQ3LBw";

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeInvalidIfNotParsed()
	{
		const string token =
			"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
			"eyJhcHoiOiI3ZDc3NWYxNy1hYWZhLTQyYTQtYTg0Mi0yYTIzZDE0MGZlZDYiLcJzbGIiOiIxIiwibmJmIjoxNjQ2NTg1NTM5LCJleHAiOjE2NDY1ODU4MzksImlhdCI6MTY0NjU4NTUzOSwiaXNzIjoiVmFsaWRJc3N1ZXIifQ." +
			"3WXikHPvJtnSxNwGG0SSScxNCzuux7oaP7dAncQ3LBw";

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeInvalidIfAnotherIssuer()
	{
		const string invalidIssuer = nameof(invalidIssuer);
		var token = GenerateToken(Guid.NewGuid(), 123L, invalidIssuer);

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeInvalidIfSignedWithAnotherKey()
	{
		const string invalidKey = nameof(invalidKey) + "1234567890";
		var token = GenerateToken(Guid.NewGuid(), 123L, signingKey: invalidKey);

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeInvalidIfNoConnectionId()
	{
		var token = GenerateToken(null, 123L);

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeInvalidIfNoUserId()
	{
		var token = GenerateToken(Guid.NewGuid(), null);

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeInvalidIfConnectionNotSet()
	{
		var tokenId = Guid.NewGuid();
		var token = GenerateToken(tokenId, 123L);

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		using var cts = new CancellationTokenSource();

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		MockAuthService.Verify(x => x.TrySetConnectionInfoAsync(It.Is<AuthSetConnectionInfoParams>(y => y.TokenId == tokenId), cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task BeValidIfConnectionSet()
	{
		const long userId = 123L;
		Guid tokenId = Guid.NewGuid(), connectionId = Guid.NewGuid();
		var token = GenerateToken(tokenId, userId);

		var input = new AuthConnectionValidationParams
		{
			Token = token,
			IpAddress = "192.168.102.78",
			ClientInfo = "Microsoft Edge 98"
		};

		var expected = new AuthConnectionValidationResult
		{
			ConnectionId = connectionId,
			UserId = userId
		};

		var expectedParams = new AuthSetConnectionInfoParams(tokenId, userId)
		{
			IpAddress = input.IpAddress,
			ClientInfo = input.ClientInfo
		};

		using var cts = new CancellationTokenSource();

		MockAuthService
			.Setup(x => x.TrySetConnectionInfoAsync(It.Is<AuthSetConnectionInfoParams>(y => y.TokenId == tokenId), cts.Token))
			.ReturnsAsync(connectionId);

		var result = await CreateFixture()
			.ValidateRefreshTokenAsync(input, cts.Token);

		result
			.Should()
			.Be(expected);

		MockAuthService.Verify(x => x.TrySetConnectionInfoAsync(expectedParams, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}