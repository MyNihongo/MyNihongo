using System.Collections.Concurrent;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyNihongo.WebApi.Models;

namespace MyNihongo.WebApi.Utils.Extensions;

public static class ConfigurationEx
{
	private const string JwtSection = "Jwt",
		AccessTokenSection = "AccessToken",
		RefreshTokenSection = "RefreshToken";

	private static readonly ConcurrentDictionary<byte, Lazy<ConfigurationTokenOptions>> TokenOptionsCache = new();

	public static ConfigurationTokenOptions GetAccessTokenOptions(this IConfiguration @this) =>
		@this.GetTokenOptions(1, AccessTokenSection, 15d); // 15 minutes

	public static ConfigurationTokenOptions GetRefreshTokenOptions(this IConfiguration @this) =>
		@this.GetTokenOptions(2, RefreshTokenSection, 129_600d); // 90 days

	public static string GetValidIssuer(this IConfiguration @this) =>
		@this.GetValue<string?>($"{JwtSection}:ValidIssuer") ?? throw new InvalidOperationException("ValidIssuer is not defined");

	private static ConfigurationTokenOptions GetTokenOptions(this IConfiguration @this, in byte key, string prop, double defaultExpiration) =>
		TokenOptionsCache.GetOrAdd(key, _ =>
		{
			return new Lazy<ConfigurationTokenOptions>(() =>
			{
				return new ConfigurationTokenOptions(
					@this.GetExpiration(prop, defaultExpiration),
					@this.GetSecurityKey(prop)
				);
			});
		}).Value;

	private static SecurityKey GetSecurityKey(this IConfiguration @this, string prop)
	{
		var keyString = @this.GetValue($"{JwtSection}:{prop}:SigningKey", string.Empty);
		if (string.IsNullOrEmpty(keyString))
			throw new InvalidOperationException("SigningKey is not defined");

		var bytes = Encoding.UTF8.GetBytes(keyString);
		return new SymmetricSecurityKey(bytes);
	}

	private static TimeSpan GetExpiration(this IConfiguration @this, string prop, double defaultValue)
	{
		var doubleValue = @this.GetValue($"{JwtSection}:{prop}:ExpirationMinutes", defaultValue);
		return TimeSpan.FromMinutes(doubleValue);
	}
}