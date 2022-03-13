using System.Diagnostics.CodeAnalysis;
using MyNihongo.WebApi.Models.Auth;

namespace MyNihongo.WebApi.Auth;

public interface IUserSession
{
	long? UserId { get; }

	long RequiredUserId => UserId ?? throw new InvalidOperationException("Unauthorised");

	bool TryGetTokenData([NotNullWhen(true)] out AuthGenerateTokensResult? tokenData);
}