namespace MyNihongo.WebApi.Infrastructure.Kanji;

public sealed partial class KanjiGetDetailRequest : IRequest<KanjiGetDetailResponse>
{
	public long? UserId { get; init; }
}