namespace MyNihongo.SrcGen.Migrations.Utils.Extensions;

internal static class TypeSymbolEx
{
	public static IEnumerable<IPropertySymbol> GetProperties(this ITypeSymbol @this)
	{
		while (true)
		{
			var props = @this.GetMembers()
				.OfType<IPropertySymbol>()
				.Where(static x => x.DeclaredAccessibility == Accessibility.Public);

			foreach (var prop in props)
				yield return prop;

			if (@this.BaseType == null)
				yield break;

			@this = @this.BaseType;
		}
	}

	public static bool TryGetEnumUnderlyingType(this ITypeSymbol @this, out INamedTypeSymbol? underlying)
	{
		underlying = null;

		if (@this is INamedTypeSymbol namedTypeSymbol)
			underlying = namedTypeSymbol.EnumUnderlyingType;

		return underlying != null;
	}
}