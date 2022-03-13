using MyNihongo.WebApi.Models.Auth;

namespace MyNihongo.WebApi.Auth;

public interface IMutableUserSession : IUserSession
{
	void SetUserId(long userId);

	void SetTokens(AuthGenerateTokensResult value);
}