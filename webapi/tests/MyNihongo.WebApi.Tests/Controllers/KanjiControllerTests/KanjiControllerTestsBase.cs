namespace MyNihongo.WebApi.Tests.Controllers.KanjiControllerTests;

public abstract class KanjiControllerTestsBase : ControllerTestsBase
{
	protected KanjiController CreateFixture() =>
		new(MockMediator.Object, MockUserSession.Object);
}