namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class PropertySymbolEx
{
	public static bool IsPropEnumerable(this IPropertySymbol prop)
	{
		if (prop.Type.Name == "String")
			return false;

		return prop.Type.Implements("IEnumerable");
	}
}