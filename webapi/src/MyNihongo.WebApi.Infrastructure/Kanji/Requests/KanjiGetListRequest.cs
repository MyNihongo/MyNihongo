namespace MyNihongo.WebApi.Infrastructure.Kanji;

public sealed partial class KanjiGetListRequest : IStreamRequest<KanjiGetListResponse>
{
	public long? UserId { get; init; }
}