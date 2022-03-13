namespace MyNihongo.WebApi.Infrastructure;

public static class InstantEx
{
	public static long GetTicksWithOffset(this Instant @this, in TimeSpan offset) =>
		@this.Plus(offset.ToDuration())
			.ToUnixTimeTicks();
}