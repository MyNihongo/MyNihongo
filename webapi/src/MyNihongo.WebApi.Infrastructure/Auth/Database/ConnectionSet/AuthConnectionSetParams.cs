namespace MyNihongo.WebApi.Infrastructure.Auth;

[StoredProcedureContext("spAuthConnectionSet", typeof(AuthConnectionSetResult), IsNullable = true)]
internal sealed record AuthConnectionSetParams
{
	[Param("tokenID")]
	public Guid TokenId { get; init; }

	[Param("userID")]
	public long UserId { get; init; }

	[Param("ipAddress")]
	public string IpAddress { get; init; } = string.Empty;

	[Param("clientInfo")]
	public string ClientInfo { get; init; } = string.Empty;

	[Param("tickNow")]
	public long TicksNow { get; init; }
}