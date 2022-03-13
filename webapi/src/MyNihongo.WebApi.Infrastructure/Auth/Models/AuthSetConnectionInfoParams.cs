namespace MyNihongo.WebApi.Infrastructure.Auth;

public sealed record AuthSetConnectionInfoParams(Guid TokenId, long UserId)
{
	public string IpAddress { get; init; } = string.Empty;

	public string ClientInfo { get; init; } = string.Empty;
}