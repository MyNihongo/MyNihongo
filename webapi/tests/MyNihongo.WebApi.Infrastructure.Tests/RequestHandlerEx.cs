using MediatR;

namespace MyNihongo.WebApi.Infrastructure.Tests;

internal static class RequestHandlerEx
{
	public static async ValueTask<TResponse[]> HandleAsync<TRequest, TResponse>(this IStreamRequestHandler<TRequest, TResponse> @this, TRequest request, CancellationToken ct)
		where TRequest : IStreamRequest<TResponse>
	{
		var list = new List<TResponse>();

		await foreach (var item in @this.Handle(request, ct))
			list.Add(item);

		return list.ToArray();
	}
}