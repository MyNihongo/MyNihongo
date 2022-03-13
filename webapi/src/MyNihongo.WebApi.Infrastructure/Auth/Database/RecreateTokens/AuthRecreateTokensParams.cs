namespace MyNihongo.WebApi.Infrastructure.Auth;

[StoredProcedureContext("spAuthRecreateTokens", typeof(AuthRecreateTokensResult))]
public sealed record AuthRecreateTokensParams
{
	[Param("connectionID")]
	public Guid ConnectionId { get; init; }

	[Param("ticksAccessValidTo")]
	public long TicksAccessValidTo { get; init; }

	[Param("ticksRefreshValidTo")]
	public long TicksRefreshValidTo { get; init; }
}