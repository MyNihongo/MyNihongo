namespace MyNihongo.SrcGen.Migrations.Utils.Extensions;

internal static class AttributeDataEx
{
	public static string GetName(this AttributeData @this)
	{
		var name = @this.AttributeClass?.Name ?? string.Empty;

		var trimIndex = name.LastIndexOf("Attribute", StringComparison.OrdinalIgnoreCase);
		if (trimIndex != -1)
			name = name.Substring(0, trimIndex);

		return name;
	}
}