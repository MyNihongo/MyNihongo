﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyNihongo.Tests.Integration;

public static class ObjectApprovals
{
	private static readonly JsonSerializer JsonSerializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
	{
		Converters = new JsonConverter[]
		{
			new StringEnumConverter()
		}
	});

	static ObjectApprovals()
	{
		VerifierSettings.DerivePathInfo((_, projectDirectory, type, method) =>
		{
			var basePath = GetExecutionPath(projectDirectory, type);
			basePath = Path.Combine(basePath, "Approvals");

			return new PathInfo(basePath, type.Name, method.Name);
		});

		VerifierSettings.UseStrictJson();
	}

	public static async Task<string> GetJsonAsync<T>(this IAsyncEnumerable<T> @this)
	{
		var obj = await @this.ToArrayAsync()
			.ConfigureAwait(false);

		return GetJsonString(obj);
	}

	public static string GetJson<T>(this T[] @this) =>
		GetJsonString(@this);

	public static async Task<string> GetJsonAsync<T>(this Task<T> @this)
	{
		var obj = await @this.ConfigureAwait(false);
		return GetJsonString(obj);
	}

	internal static string GetJsonString(object? obj)
	{
		if (obj == null)
			return string.Empty;

		using var stringWriter = new StringWriter();
		using var jsonWriter = new JsonTextWriter(stringWriter)
		{
			Formatting = Formatting.Indented,
			IndentChar = '\t',
			Indentation = 1
		};

		JsonSerializer.Serialize(jsonWriter, obj);

		return stringWriter.ToString();
	}

	private static string GetExecutionPath(string projectDirectory, Type classType)
	{
		if (string.IsNullOrEmpty(classType.Namespace))
			return projectDirectory;

		var @namespace = classType.Namespace;
		if (projectDirectory.EndsWith(Path.DirectorySeparatorChar))
			projectDirectory = Path.GetDirectoryName(projectDirectory)!;

		var rootNamespace = Path.GetFileName(projectDirectory);
		if (@namespace.StartsWith(rootNamespace))
			@namespace = @namespace[(rootNamespace.Length + 1)..]; // +1 for the trailing `.`

		var dirs = @namespace.Split('.')
			.Prepend(projectDirectory)
			.ToArray();

		return Path.Combine(dirs);
	}
}