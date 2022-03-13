namespace MyNihongo.Tests.GrpcClient;

public static class GrpcApprovals
{
	public static async Task<string> GetJsonAsync<T>(this AsyncUnaryCall<T> @this)
	{
		var obj = new
		{
			payload = await @this.ResponseAsync
				.ConfigureAwait(false),

			headers = @this.GetTrailers()
				.ScrubData()
		};

		return ObjectApprovals.GetJsonString(obj);
	}

	public static async Task<string> GetJsonAsync<T>(this AsyncServerStreamingCall<T> @this)
	{
		var obj = new
		{
			payload = await @this.ResponseStream
				.ReadAllAsync()
				.ToArrayAsync(),

			headers = @this.GetTrailers()
				.ScrubData()
		};

		return ObjectApprovals.GetJsonString(obj);
	}

	private static IReadOnlyList<KeyValuePair<string, string>> ScrubData(this Metadata metadata)
	{
		if (metadata.Count == 0)
			return Array.Empty<KeyValuePair<string, string>>();

		var arr = new KeyValuePair<string, string>[metadata.Count];

		for (var i = 0; i < metadata.Count; i++)
		{
			var value = metadata[i].Value;
			if (metadata[i].Key == "set-cookie")
				value = ScrubJwtData(value);

			arr[i] = new KeyValuePair<string, string>(metadata[i].Key, value);
		}

		return arr;
	}

	private static string ScrubJwtData(string tokenHeaderValue)
	{
		var start = tokenHeaderValue.IndexOf('=');
		if (start == -1)
			return tokenHeaderValue;

		start++;
		var end = tokenHeaderValue.IndexOf(';', start);
		if (end == -1 || end == start)
			return tokenHeaderValue;

		var startValue = tokenHeaderValue[..start];
		var endValue = tokenHeaderValue[end..];

		return startValue + "{token value}" + endValue;
	}
}