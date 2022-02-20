namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class SymbolEx
{
	public static bool TryGetAttrValue<T>(this ISymbol @this, string attrName, int ctorParamIndex, out T value)
	{
		value = default!;

		var attrs = @this.GetAttributes();
		for (var i = 0; i < attrs.Length; i++)
		{
			if (!attrs[i].IsNameEqual(attrName) || attrs[i].ConstructorArguments.Length <= ctorParamIndex)
				continue;

			if (attrs[i].ConstructorArguments[ctorParamIndex].Value is T tValue)
			{
				value = tValue;
				return true;
			}

			if (Nullable.GetUnderlyingType(typeof(T)) != null)
				return true;

			break;
		}

		return false;
	}

	public static bool TryGetAttrValue<T>(this ISymbol @this, string attrName, string paramName, out T value)
	{
		value = default!;

		var attrs = @this.GetAttributes();
		for (var i = 0; i < attrs.Length; i++)
		{
			if (!attrs[i].IsNameEqual(attrName))
				continue;

			for (var j = 0; j < attrs[i].NamedArguments.Length; j++)
			{
				if (attrs[i].NamedArguments[j].Key != paramName)
					continue;

				if (attrs[i].NamedArguments[j].Value.Value is T tValue)
				{
					value = tValue;
					return true;
				}

				goto Return;
			}
		}

		Return:
		return false;
	}
}