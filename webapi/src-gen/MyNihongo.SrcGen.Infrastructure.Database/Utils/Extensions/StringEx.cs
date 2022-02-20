namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class StringEx
{
	public static string ToPascalCase(this string @this)
	{
		if (string.IsNullOrEmpty(@this))
			return string.Empty;

		return char.ToLower(@this![0]) + @this.Substring(1);
	}
}