namespace MyNihongo.Tests.Integration.Models.Auth;

[Table(Tables.Auth.Connection)]
public sealed record AuthConnectionDatabaseRecord
{
	[Column(Connection.ConnectionId, IsPrimaryKey = true)]
	public Guid ConnectionId { get; init; }

	[Column(Connection.UserId)]
	public long UserId { get; init; }

	[Column(Connection.IpAddress, CanBeNull = false)]
	public string IpAddress { get; init; } = string.Empty;

	[Column(Connection.ClientInfo, CanBeNull = false)]
	public string ClientInfo { get; init; } = string.Empty;

	[Column(Connection.TicksLatestAccessed)]
	public long TicksLatestAccess { get; init; }
}