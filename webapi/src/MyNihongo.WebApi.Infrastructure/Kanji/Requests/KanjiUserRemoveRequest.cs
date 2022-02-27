namespace MyNihongo.WebApi.Infrastructure.Kanji;

public sealed partial class KanjiUserRemoveRequest : IRequest<KanjiUserRemoveResponse>
{
	public long UserId { get; init; }
}