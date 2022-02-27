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

	public override Task<KanjiGetDetailResponse> GetDetail(KanjiGetDetailRequest request, ServerCallContext context)
	{
		return _mediator.Send(request, context.CancellationToken);
	}

	public override Task GetList(KanjiGetListRequest request, IServerStreamWriter<KanjiGetListResponse> responseStream, ServerCallContext context)
	{
		return _mediator.CreateStream(request, context.CancellationToken)
			.WriteTo(responseStream, context);
	}

	//[Authorize]
	public override Task<KanjiUserAddResponse> UserAdd(KanjiUserAddRequest request, ServerCallContext context)
	{
		return _mediator.Send(request, context.CancellationToken);
	}

	//[Authorize]
	public override Task<KanjiUserRemoveResponse> UserRemove(KanjiUserRemoveRequest request, ServerCallContext context)
	{
		return _mediator.Send(request, context.CancellationToken);
	}

	//[Authorize]
	public override Task<KanjiUserSetResponse> UserSet(KanjiUserSetRequest request, ServerCallContext context)
	{
		return _mediator.Send(request, context.CancellationToken);
	}
}