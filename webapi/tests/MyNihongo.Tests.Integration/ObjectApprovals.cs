using ApprovalTests;
using Newtonsoft.Json;
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

	public static void VerifyJson(object obj)
	{
		using var stringWriter = new StringWriter();
		using var jsonWriter = new JsonTextWriter(stringWriter)
		{
			Formatting = Formatting.Indented,
			IndentChar = '\t',
			Indentation = 1
		};

		JsonSerializer.Serialize(jsonWriter, obj);

		var json = stringWriter.ToString();
		Approvals.VerifyWithExtension(json, ".json");
	}
}