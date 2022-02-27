namespace MyNihongo.WebApi.Infrastructure.Kanji;

public sealed partial class KanjiUserSetRequest : IRequest<KanjiUserSetResponse>
{
	public long UserId { get; init; }
}