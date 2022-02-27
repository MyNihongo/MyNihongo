namespace MyNihongo.WebApi.Infrastructure;

internal static class ClockEx
{
	public static long GetTicksNow(this IClock @this) =>
		@this.GetCurrentInstant().ToUnixTimeTicks();
}