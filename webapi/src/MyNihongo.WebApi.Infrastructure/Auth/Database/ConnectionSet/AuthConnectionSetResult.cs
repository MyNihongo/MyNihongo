namespace MyNihongo.WebApi.Infrastructure.Auth;

internal sealed record AuthConnectionSetResult
{
	public Guid ConnectionId { get; init; }
}