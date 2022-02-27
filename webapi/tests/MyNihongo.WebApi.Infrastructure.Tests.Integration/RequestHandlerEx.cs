using MediatR;

namespace MyNihongo.WebApi.Infrastructure.Tests.Integration;

internal static class RequestHandlerEx
{
	public static Task<TResponse> HandleAsync<TRequest, TResponse>(this IRequestHandler<TRequest, TResponse> @this, TRequest request, CancellationToken ct = default)
		where TRequest : IRequest<TResponse>
	{
		return @this.Handle(request, ct);
	}

	public static ValueTask<TResponse[]> HandleAsync<TRequest, TResponse>(this IStreamRequestHandler<TRequest, TResponse> @this, TRequest request, CancellationToken ct = default)
		where TRequest : IStreamRequest<TResponse>
	{
		return @this.Handle(request, ct)
			.ToArrayAsync(ct);
	}

	public static Task<string> HandleJsonAsync<TRequest, TResponse>(this IRequestHandler<TRequest, TResponse> @this, TRequest request, CancellationToken ct = default)
		where TRequest : IRequest<TResponse>
	{
		return @this.Handle(request, ct)
			.GetJsonAsync();
	}

	public static Task<string> HandleJsonAsync<TRequest, TResponse>(this IStreamRequestHandler<TRequest, TResponse> @this, TRequest request, CancellationToken ct = default)
		where TRequest : IStreamRequest<TResponse>
	{
		return @this.Handle(request, ct)
			.GetJsonAsync();
	}
}