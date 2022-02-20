namespace MyNihongo.SrcGen.Infrastructure.Database.Utils.Extensions;

internal static class StringBuilderEx
{
	public static StringBuilder AppendNamespace(this StringBuilder @this, Compilation compilation, string? suffix = null)
	{
		var isTestRunner = compilation.AssemblyName == "TestProject";

		var @namespace = string.IsNullOrEmpty(compilation.AssemblyName) || isTestRunner
			? "MyNihongo.WebApi.Infrastructure"
			: compilation.AssemblyName!;

		if (!string.IsNullOrEmpty(suffix))
			@namespace += "." + suffix;

		if (isTestRunner)
		{
			@this
				.AppendLine("using System;")
				.AppendLine("using System.Data;")
				.AppendLine("using System.Collections.Generic;")
				.AppendLine("using System.Threading;")
				.AppendLine("using System.Threading.Tasks;")
				.AppendLine("using Microsoft.Data.SqlClient;")
				.AppendLine("using Microsoft.Extensions.DependencyInjection;")
				.AppendLine("using MyNihongo.Database.Abstractions;")
				.AppendLine();
		}

		return @this
			.AppendFormat("namespace {0};", @namespace).AppendLine()
			.AppendLine();
	}

	public static StringBuilder Indent(this StringBuilder @this, int indent)
	{
		for (var i = 0; i < indent; i++)
			@this.Append('\t');

		return @this;
	}
}