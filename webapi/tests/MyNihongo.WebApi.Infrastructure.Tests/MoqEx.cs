namespace MyNihongo.WebApi.Infrastructure.Tests;

internal static class MoqEx
{
	public static void SetupInstant(this Mock<IClock> @this, int year, int monthOfYear, int dayOfMonth, int hourOfDay)
	{
		@this.Setup(x => x.GetCurrentInstant())
			.Returns(Instant.FromUtc(year, monthOfYear, dayOfMonth, hourOfDay, 0));
	}
}