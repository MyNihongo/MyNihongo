using MyNihongo.WebApi.Infrastructure.Kanji;
using MyNihongo.WebApi.Utils.Extensions;

namespace MyNihongo.WebApi.Services;

public sealed class KanjiController : KanjiRpc.KanjiRpcBase
{
	private readonly IMediator _mediator;

	public KanjiController(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override Task GetList(KanjiGetListRequest request, IServerStreamWriter<KanjiGetListResponse> responseStream, ServerCallContext context) =>
		_mediator.CreateStream(request, context.CancellationToken)
			.WriteTo(responseStream, context);
}