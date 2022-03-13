namespace MyNihongo.Tests.Integration.Models.Kanji;

[Table(Tables.Kanji.UserEntry)]
public sealed record KanjiUserEntryDatabaseRecord
{
	[Column(UserEntry.UserId, IsPrimaryKey = true, PrimaryKeyOrder = 0)]
	public long UserId { get; init; }

	[Column(UserEntry.KanjiId, IsPrimaryKey = true, PrimaryKeyOrder = 1)]
	public short KanjiId { get; init; }

	[Column(UserEntry.FavouriteRating)]
	public byte FavouriteRating { get; init; }

	[Column(UserEntry.Notes, CanBeNull = false)]
	public string Notes { get; init; } = string.Empty;

	[Column(UserEntry.Mark)]
	public byte Mark { get; init; }

	[Column(UserEntry.IsDeleted)]
	public bool IsDeleted { get; init; }

	[Column(UserEntry.TicksLatestAccess)]
	public long TicksLatestAccess { get; init; }

	[Column(UserEntry.TicksModified)]
	public long TicksModified { get; init; }
}