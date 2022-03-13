using MyNihongo.WebApi.Auth;
using MyNihongo.WebApi.Infrastructure.Kanji;
using MyNihongo.WebApi.Utils.Extensions;

namespace MyNihongo.WebApi;

public sealed class KanjiController : KanjiRpc.KanjiRpcBase
{
	private readonly IMediator _mediator;
	private readonly IUserSession _userSession;

	public KanjiController(
		IMediator mediator,
		IUserSession userSession)
	{
		_mediator = mediator;
		_userSession = userSession;
	}

	public override Task<KanjiGetDetailResponse> GetDetail(KanjiGetDetailRequest request, ServerCallContext context)
	{
		request = new KanjiGetDetailRequest(request)
		{
			UserId = _userSession.UserId
		};

		return _mediator.Send(request, context.CancellationToken);
	}

	public override Task GetList(KanjiGetListRequest request, IServerStreamWriter<KanjiGetListResponse> responseStream, ServerCallContext context)
	{
		request = new KanjiGetListRequest(request)
		{
			UserId = _userSession.UserId
		};

		return _mediator.CreateStream(request, context.CancellationToken)
			.WriteTo(responseStream, context);
	}

	[Authorize]
	public override Task<KanjiUserAddResponse> UserAdd(KanjiUserAddRequest request, ServerCallContext context)
	{
		request = new KanjiUserAddRequest(request)
		{
			UserId = _userSession.RequiredUserId
		};

		return _mediator.Send(request, context.CancellationToken);
	}

	[Authorize]
	public override Task<KanjiUserRemoveResponse> UserRemove(KanjiUserRemoveRequest request, ServerCallContext context)
	{
		request = new KanjiUserRemoveRequest(request)
		{
			UserId = _userSession.RequiredUserId
		};

		return _mediator.Send(request, context.CancellationToken);
	}

	[Authorize]
	public override Task<KanjiUserSetResponse> UserSet(KanjiUserSetRequest request, ServerCallContext context)
	{
		request = new KanjiUserSetRequest(request)
		{
			UserId = _userSession.RequiredUserId
		};

		return _mediator.Send(request, context.CancellationToken);
	}
}