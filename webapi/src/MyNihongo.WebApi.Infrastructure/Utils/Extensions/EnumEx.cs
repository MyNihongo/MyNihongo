namespace MyNihongo.WebApi.Infrastructure;

internal static class EnumEx
{
	public static T? NullIfDefault<T>(this T @this)
		where T : struct, Enum
	{
		return @this.IsDefault() ? null : @this;
	}
}