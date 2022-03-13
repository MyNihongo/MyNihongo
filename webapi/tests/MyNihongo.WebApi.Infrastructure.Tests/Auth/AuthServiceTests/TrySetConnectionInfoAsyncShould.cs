// ReSharper disable AccessToDisposedClosure
namespace MyNihongo.WebApi.Infrastructure.Tests.Auth.AuthServiceTests;

public sealed class TrySetConnectionInfoAsyncShould : AuthServiceTestsBase
{
	[Fact]
	public async Task BeNullIfNotSet()
	{
		MockClock.SetupInstant(2022, 3, 6, 13);

		var input = new AuthSetConnectionInfoParams(Guid.NewGuid(), 123L);
		using var cts = new CancellationTokenSource();

		var expectedParams = new AuthConnectionSetParams
		{
			TokenId = input.TokenId,
			UserId = input.UserId,
			IpAddress = input.IpAddress,
			ClientInfo = input.ClientInfo,
			TicksNow = 16465716000000000L
		};

		var result = await CreateFixture()
			.TrySetConnectionInfoAsync(input, cts.Token);

		result
			.Should()
			.BeNull();

		MockDatabase.Verify(x => x.ConnectionSetAsync(expectedParams, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}

	[Fact]
	public async Task ReturnConnectionId()
	{
		MockClock.SetupInstant(2022, 3, 6, 13);

		var connectionId = Guid.NewGuid();
		var input = new AuthSetConnectionInfoParams(Guid.NewGuid(), 123L);
		using var cts = new CancellationTokenSource();

		var expectedParams = new AuthConnectionSetParams
		{
			TokenId = input.TokenId,
			UserId = input.UserId,
			IpAddress = input.IpAddress,
			ClientInfo = input.ClientInfo,
			TicksNow = 16465716000000000L
		};

		MockDatabase
			.Setup(x => x.ConnectionSetAsync(expectedParams, cts.Token))
			.ReturnsAsync(new AuthConnectionSetResult { ConnectionId = connectionId });

		var result = await CreateFixture()
			.TrySetConnectionInfoAsync(input, cts.Token);

		result
			.Should()
			.Be(connectionId);

		MockDatabase.Verify(x => x.ConnectionSetAsync(expectedParams, cts.Token), Times.Once);
		VerifyNoOtherCalls();
	}
}