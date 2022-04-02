using MyNihongo.WebApi.Auth;
using MyNihongo.WebApi.Infrastructure.Auth;

namespace MyNihongo.WebApi;

public sealed class AuthController : AuthRpc.AuthRpcBase
{
	private readonly IMediator _mediator;
	private readonly IUserSession _userSession;

	public AuthController(
		IMediator mediator,
		IUserSession userSession)
	{
		_mediator = mediator;
		_userSession = userSession;
	}

	public override Task<AuthSignInResponse> SignIn(AuthSignInRequest request, ServerCallContext context)
	{
		return base.SignIn(request, context);
	}

	public override Task<AuthSignUpResponse> SignUp(AuthSignUpRequest request, ServerCallContext context)
	{
		return base.SignUp(request, context);
	}

	public override Task<AuthSignUpConfirmationResponse> ConfirmSignUp(AuthSignUpConfirmationRequest request, ServerCallContext context)
	{
		return base.ConfirmSignUp(request, context);
	}

	[Authorize]
	public override Task<AuthSignOutResponse> SignOut(AuthSignOutRequest request, ServerCallContext context)
	{
		return base.SignOut(request, context);
	}
}