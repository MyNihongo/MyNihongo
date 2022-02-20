namespace MyNihongo.WebApi.Utils.Extensions;

public static class AsyncEnumerableEx
{
	public static async Task WriteTo<T>(this IAsyncEnumerable<T> @this, IServerStreamWriter<T> responseStream, ServerCallContext ctx)
	{
		await foreach (var res in @this.WithCancellation(ctx.CancellationToken))
			await responseStream.WriteAsync(res)
				.ConfigureAwait(false);
	}
}