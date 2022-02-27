namespace MyNihongo.WebApi.Infrastructure;

internal static class ByteEx
{
	public static JlptLevel ToJlptLevel(in this byte? @this) =>
		@this.HasValue ? (JlptLevel)@this.Value : JlptLevel.UndefinedJlptLevel;

	public static double ToFavouriteRating(in this byte @this) =>
		Math.Round(@this / 10d, 1);
}