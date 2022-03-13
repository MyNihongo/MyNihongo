namespace MyNihongo.WebApi.Infrastructure.Tests.Auth.AuthServiceTests;

public abstract class AuthServiceTestsBase
{
	internal Mock<IAuthDatabaseService> MockDatabase { get; } = new();

	protected Mock<IClock> MockClock { get; } = new();

	protected IAuthService CreateFixture() =>
		new AuthService(
			MockDatabase.Object,
			MockClock.Object);

	public void VerifyNoOtherCalls()
	{
		MockDatabase.VerifyNoOtherCalls();
	}
}