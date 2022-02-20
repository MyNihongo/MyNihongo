namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class AttributeDataEx
{
	public static bool IsNameEqual(this AttributeData @this, string attrName)
	{
		const string attribute = "Attribute";
		var actualName = @this.AttributeClass?.Name ?? string.Empty;
		var lastIndex = actualName.LastIndexOf(attribute, StringComparison.Ordinal);

		if (lastIndex <= 0)
			return false;

		for (var i = 0; i < lastIndex; i++)
			if (i >= attrName.Length || attrName[i] != actualName[i])
				return false;

		return true;
	}
}