namespace MyNihongo.SrcGen.Migrations.Utils.Extensions;

internal static class StringBuilderEx
{
	public static StringBuilder AppendNamespace(this StringBuilder @this, Compilation compilation)
	{
		var @namespace = string.IsNullOrEmpty(compilation.AssemblyName)
			? "MyNihongo"
			: compilation.AssemblyName;

		return @this
			.AppendFormat("namespace {0};", @namespace)
			.AppendLine();
	}
}