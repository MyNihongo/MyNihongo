using System.Reflection;
using Microsoft.CodeAnalysis.Testing;

namespace MyNihongo.SrcGen.Infrastructure.Database.Tests.DatabaseGeneratorTests;

public abstract class DatabaseGeneratorTestsBase
{
#if DEBUG
	private const string Configuration = "Debug";
#else
	private const string Configuration = "Release";
#endif

	private const string Runtime = "net6.0";

	protected const string TestUsings =
@"using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using MyNihongo.Database.Abstractions;

";

	private static readonly ObjectPool<StringBuilder> StringBuilderPool = new DefaultObjectPoolProvider()
		.CreateStringBuilderPool();

	private static readonly string[] ImplicitUsings = { "System", "System.Linq", "System.Collections.Generic", "System.Threading", "System.Threading.Tasks" },
		ProtoDirs = { "enums", "messages" },
		UtilsDirs = { "Collections", "Extensions" },
		GeneratorDirs = { "Database", "Extensions" };

	private static readonly ImmutableArray<PackageIdentity> NuGetPackages = ImmutableArray.Create(
		new PackageIdentity("Google.Protobuf", "3.19.4"),
		new PackageIdentity("Microsoft.Data.SqlClient", "4.1.0"),
		new PackageIdentity("Microsoft.Extensions.Configuration.Abstractions", "6.0.0"),
		new PackageIdentity("MediatR.Extensions.Microsoft.DependencyInjection", "10.0.1"),
		new PackageIdentity("NodaTime", "3.0.9")
	);

	private static readonly ImmutableArray<string> Projects = ImmutableArray.Create(
		"MyNihongo.Database.Abstractions"
	);

	protected static async Task VerifyGeneratorAsync(GeneratorKey key)
	{
		const string expectedAttrs = TestUsings +
@"namespace MyNihongo.WebApi.Infrastructure;

#nullable enable
[AttributeUsage(AttributeTargets.Class)]
internal sealed class StoredProcedureContextAttribute : Attribute
{
	public StoredProcedureContextAttribute(string storedProcedureName, Type? returnType) { }

	public bool IsNullable { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
internal sealed class ParamAttribute : Attribute
{
	public ParamAttribute(string parameterName) { }
}

[AttributeUsage(AttributeTargets.Property)]
internal sealed class ParamTempTableAttribute : Attribute
{
	public ParamTempTableAttribute(string tempTableName) { }

	public string? Collation { get; init; }
}
#nullable disable
";

		var expectedServiceCollection = TestUsings +
$@"namespace MyNihongo.WebApi.Infrastructure;

internal static class ServiceCollectionForDatabaseEx
{{
	public static IServiceCollection AddDatabaseServices(this IServiceCollection @this) =>
		@this
			.AddSingleton<{key}.I{key}DatabaseService, {key}.{key}DatabaseService>();
}}
";

		var expected = await ResultUtils.LoadExpectedAsync(key, TestUsings)
			.ConfigureAwait(false);

		var runner = new GeneratorRunner
		{
			TestState =
			{
				GeneratedSources =
				{
					(typeof(DatabaseGenerator), "Attributes.g.cs", expectedAttrs),
					(typeof(DatabaseGenerator), $"{key}DatabaseService.g.cs", expected),
					(typeof(DatabaseGenerator), "ServiceCollectionEx.g.cs", expectedServiceCollection)
				}
			},
			ReferenceAssemblies = ReferenceAssemblies.Default.AddPackages(NuGetPackages)
		};

		foreach (var assembly in LoadAdditionalReferences())
			runner.TestState.AdditionalReferences.Add(assembly);

		await foreach (var source in LoadSourceFiles(key))
			runner.TestState.Sources.Add(source);

		await runner.RunAsync();
	}

	private static IEnumerable<Assembly> LoadAdditionalReferences()
	{
		return Directory.EnumerateFiles(AppContext.BaseDirectory, "MyNihongo.*.dll")
			.Where(x =>
			{
				var fileName = Path.GetFileNameWithoutExtension(x);
				return Projects.Contains(fileName);
			})
			.Select(Assembly.LoadFile);
	}

	private static async IAsyncEnumerable<(string, SourceText)> LoadSourceFiles(GeneratorKey key)
	{
		var path = AppContext.BaseDirectory;
		do
		{
			path = Path.GetDirectoryName(path);
		} while (path != null && !path.EndsWith("webapi"));

		if (string.IsNullOrEmpty(path))
			throw new InvalidOperationException("Cannot find the root path");

		path = Path.Combine(path, "src", "MyNihongo.WebApi.Infrastructure");

		// Init declaration
		yield return LoadInit();

		// Base types
		await foreach (var file in LoadFilesAsync(path, SearchOption.TopDirectoryOnly))
			yield return file;

		// Proto files
		var resourcePath = Path.Combine(path, "obj", Configuration, Runtime);
		await foreach (var file in LoadFilesAsync(resourcePath, ProtoDirs))
			yield return file;

		// Utils
		resourcePath = Path.Combine(path, "Utils");
		await foreach (var file in LoadFilesAsync(resourcePath, UtilsDirs))
			yield return file;

		// Files for tests
		resourcePath = Path.Combine(path, key.ToString());
		await foreach (var file in LoadFilesAsync(resourcePath, GeneratorDirs))
			yield return file;
	}

	private static async IAsyncEnumerable<(string, SourceText)> LoadFilesAsync(string path, IEnumerable<string> dirs)
	{
		foreach (var dir in dirs)
		{
			var finalDir = Path.Combine(path, dir);
			if (!Directory.Exists(finalDir))
				continue;
			
			await foreach (var file in LoadFilesAsync(finalDir))
				yield return file;
		}
	}

	private static async IAsyncEnumerable<(string, SourceText)> LoadFilesAsync(string path, SearchOption searchOption = SearchOption.AllDirectories)
	{
		foreach (var file in Directory.EnumerateFiles(path, "*.cs", searchOption))
		{
			await using var fileStream = new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, 4086, true);
			using var reader = new StreamReader(fileStream);

			var name = Path.GetFileName(file);
			var content = await reader.ReadToEndAsync();
			content = AppendUsings(content);

			yield return (name, SourceText.From(content));
		}
	}

	private static (string, SourceText) LoadInit()
	{
		const string initDeclaration =
@"using System.ComponentModel;
namespace System.Runtime.CompilerServices;

[EditorBrowsable(EditorBrowsableState.Never)]
internal static class IsExternalInit {}";

		return ("Init.g.cs", SourceText.From(initDeclaration));
	}

	private static string AppendUsings(string content)
	{
		var startIndex = content.IndexOf("namespace", StringComparison.Ordinal);

		if (startIndex == -1)
			return content;

		var stringBuilder = StringBuilderPool.Get()
			.Append(content);

		// NewLine
		stringBuilder.Insert(startIndex, Environment.NewLine);

		for (var i = 0; i < ImplicitUsings.Length; i++)
		{
			var value = "using " + ImplicitUsings[i] + ';';

			stringBuilder
				.Insert(startIndex, value)
				.Insert(startIndex + value.Length, Environment.NewLine);
		}

		return stringBuilder.ToString();
	}

	private sealed class GeneratorRunner : CSharpSourceGeneratorTest<DatabaseGenerator, XUnitVerifier>
	{
		protected override ParseOptions CreateParseOptions()
		{
			var opts = (CSharpParseOptions)base.CreateParseOptions();
			return opts.WithLanguageVersion(LanguageVersion.Latest);
		}

		protected override CompilationOptions CreateCompilationOptions()
		{
			var opts = (CSharpCompilationOptions)base.CreateCompilationOptions();
			var diagnostics = opts.SpecificDiagnosticOptions.SetItems(GetNullableWarningsFromCompiler());

			return opts.WithSpecificDiagnosticOptions(diagnostics)
				.WithNullableContextOptions(NullableContextOptions.Enable)
				.WithUsings("System", "System.Data", "System.Collections.Generic", "System.Threading", "System.Threading.Tasks", "MyNihongo.Database.Abstractions");
		}

		private static IImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
		{
			string[] args = { "/warnaserror:nullable" };
			var commandLineArguments = CSharpCommandLineParser.Default.Parse(args, Environment.CurrentDirectory, Environment.CurrentDirectory);
			
			return commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;
		}
	}
}