namespace MyNihongo.WebApi.Infrastructure.Kanji;

public sealed partial class KanjiUserAddRequest : IRequest<KanjiUserAddResponse>
{
	public long UserId { get; init; }
}
