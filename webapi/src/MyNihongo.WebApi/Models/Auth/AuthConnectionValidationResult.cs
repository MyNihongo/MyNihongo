namespace MyNihongo.WebApi.Models.Auth;

public sealed record AuthConnectionValidationResult
{
	public Guid ConnectionId { get; init; }

	public long UserId { get; init; }
}