namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class TypeSymbolEx
{
	public static IEnumerable<IPropertySymbol> GetProperties(this ITypeSymbol @this, Func<IPropertySymbol, bool>? predicate = null)
	{
		while (true)
		{
			var props = @this.GetMembers()
				.OfType<IPropertySymbol>()
				.Where(static x => x.DeclaredAccessibility == Accessibility.Public);

			if (predicate != null)
				props = props.Where(predicate);

			foreach (var prop in props)
				yield return prop;

			if (@this.BaseType == null)
				yield break;

			@this = @this.BaseType;
		}
	}

	public static bool Implements(this ITypeSymbol @this, string baseType)
	{
		for (var i = 0; i < @this.AllInterfaces.Length; i++)
			if (@this.AllInterfaces[i].Name == baseType)
				return true;

		return false;
	}

	public static string GetNestedName(this ITypeSymbol @this)
	{
		var name = @this.Name;

		while (@this.ContainingType != null)
		{
			name = @this.ContainingType.Name + "." + name;
			@this = @this.ContainingType;
		}

		return name;
	}

	public static string GetDatabaseReaderMethod(this ITypeSymbol @this)
	{
		var namedType = (INamedTypeSymbol)@this;

		if (@this.NullableAnnotation == NullableAnnotation.Annotated)
		{
			var underlyingType = (INamedTypeSymbol)namedType.TypeArguments[0];
			
			return underlyingType.EnumUnderlyingType != null
				? $"EnumValue<{underlyingType.Name}?>"
				: $"NullableValue<{underlyingType}>";
		}
		if (namedType.EnumUnderlyingType != null)
		{
			return $"EnumValue<{@this.Name}>";
		}

		return @this.Name;
	}

	public static bool TryGetGenericType(this ITypeSymbol @this, int index, out INamedTypeSymbol value)
	{
		value = @this is INamedTypeSymbol namedType && namedType.TypeArguments.Length > index
			? (INamedTypeSymbol)namedType.TypeArguments[0]
			: default!;

		return value != default!;
	}

	public static bool IsSimpleType(this INamedTypeSymbol @this) =>
		!@this.IsReferenceType || @this.Name == "String";
}