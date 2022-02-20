namespace MyNihongo.WebApi.Infrastructure;

internal static class StringEx
{
	public static string TrimEx(this string? @this)
	{
		const int maxLength = 50;

		@this = @this?.Trim() ?? string.Empty;

		if (@this.Length > maxLength)
			@this = @this[..maxLength];

		return @this;
	}
}