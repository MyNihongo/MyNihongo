namespace MyNihongo.WebApi.Models.Auth;

internal sealed class AuthTokenClaimsRecord
{
	public Guid TokenId { get; private set; }

	public long UserId { get; private set; }

	public void TrySetTokenId(string value)
	{
		if (Guid.TryParse(value, out var guidValue))
			TokenId = guidValue;
	}

	public void TrySetUserId(string value)
	{
		if (long.TryParse(value, out var longValue))
			UserId = longValue;
	}

	public bool IsValid() =>
		TokenId != Guid.Empty && UserId > 0L;
}