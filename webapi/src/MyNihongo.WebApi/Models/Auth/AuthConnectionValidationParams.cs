namespace MyNihongo.WebApi.Models.Auth;

public sealed record AuthConnectionValidationParams
{
	public string Token { get; init; } = string.Empty;

	public string IpAddress { get; init; } = string.Empty;

	public string ClientInfo { get; init; } = string.Empty;
}