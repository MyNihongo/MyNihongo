using NodaTime.Extensions;

namespace MyNihongo.WebApi.Infrastructure;

public static class ClockEx
{
	public static long GetTicksNow(this IClock @this) =>
		@this.GetCurrentInstant().ToUnixTimeTicks();

	public static long GetTicksNowWithOffset(this IClock @this, TimeSpan offset) =>
		@this.GetCurrentInstant()
			.Plus(offset.ToDuration())
			.ToUnixTimeTicks();
}