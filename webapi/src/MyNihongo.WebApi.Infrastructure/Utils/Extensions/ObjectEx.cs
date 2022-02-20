namespace MyNihongo.WebApi.Infrastructure;

internal static class ObjectEx
{
	public static bool IsEqual<T>(this T @this, T that) =>
		EqualityComparer<T>.Default
			.Equals(@this, that);

	public static bool IsDefault<T>(this T @this) =>
		@this.IsEqual(default);
}