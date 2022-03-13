using System.Diagnostics.CodeAnalysis;
using MyNihongo.WebApi.Models.Auth;

namespace MyNihongo.WebApi.Auth;

public sealed class UserSession : IMutableUserSession
{
	private Optional<AuthGenerateTokensResult> _tokenDataOptional;

	public long? UserId { get; private set; }

	public bool TryGetTokenData([NotNullWhen(true)] out AuthGenerateTokensResult? tokenData)
	{
		if (_tokenDataOptional.HasValue)
		{
			tokenData = _tokenDataOptional.Value;
			return true;
		}

		tokenData = null;
		return false;
	}

	public void SetUserId(long userId) =>
		UserId = userId;

	public void SetTokens(AuthGenerateTokensResult value) =>
		_tokenDataOptional = value;
}