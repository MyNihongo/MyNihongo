namespace MyNihongo.WebApi.Infrastructure.Tests.Kanji;

public abstract class KanjiTestsBase
{
	protected Mock<IClock> MockClock { get; } = new();

	internal Mock<IKanjiDatabaseService> MockDatabase { get; } = new();

	protected void VerifyNoOtherCalls()
	{
		MockDatabase.VerifyNoOtherCalls();
	}
}