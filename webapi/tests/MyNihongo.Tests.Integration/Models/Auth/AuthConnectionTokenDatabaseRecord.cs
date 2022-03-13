namespace MyNihongo.Tests.Integration.Models.Auth;

[Table(Tables.Auth.ConnectionToken)]
public sealed record AuthConnectionTokenDatabaseRecord
{
	[Column(ConnectionToken.TokenId, IsPrimaryKey = true)]
	public Guid TokenId { get; init; }

	[Column(ConnectionToken.ConnectionId)]
	public Guid ConnectionId { get; init; }

	[Column(ConnectionToken.TicksValidTo)]
	public long TicksValidTo { get; init; }
}