namespace MyNihongo.SrcGen.Infrastructure.Database.Tests;

internal static class ResultUtils
{
	public static async Task<string> LoadExpectedAsync(GeneratorKey key, string testUsings)
	{
		var projectDir = Directory.GetParent(AppContext.BaseDirectory)
			?.Parent?.Parent?.Parent?.FullName;

		if (projectDir == null)
			throw new InvalidOperationException("Cannot determine the project directory");

		projectDir = Path.Combine(projectDir, "DatabaseGeneratorTests", $"{key}Tests", "expected.cs");

		await using var stream = new FileStream(projectDir, FileMode.Open, FileAccess.Read, FileShare.Read);
		using var reader = new StreamReader(stream);

		var result = await reader.ReadToEndAsync()
			.ConfigureAwait(false);

		var nullableDeclarationIndex = result.IndexOf("internal interface", StringComparison.OrdinalIgnoreCase);
		if (nullableDeclarationIndex == -1)
			throw new Exception("Cannot find the interface declaration");

		result = result.Insert(nullableDeclarationIndex, "#nullable enable" + Environment.NewLine);

		return new StringBuilder(testUsings)
			.Append(result)
			.AppendLine("#nullable disable")
			.ToString();
	}
}