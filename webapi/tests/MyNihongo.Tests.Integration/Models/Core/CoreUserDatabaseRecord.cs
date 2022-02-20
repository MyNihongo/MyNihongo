namespace MyNihongo.Tests.Integration.Models.Core;

[Table(Tables.Core.User)]
public sealed record CoreUserDatabaseRecord
{
	[Column(User.UserId, IsPrimaryKey = true, IsIdentity = true)]
	public long UserId { get; init; }

	[Column(User.EmailHash, CanBeNull = false)]
	public byte[] EmailHash { get; init; } = Array.Empty<byte>();

	[Column(User.PasswordHash, CanBeNull = false)]
	public byte[] PasswordHash { get; init; } = Array.Empty<byte>();
}