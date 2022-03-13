using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MyNihongo.WebApi.Models;
using MyNihongo.WebApi.Resources.Const;

namespace MyNihongo.WebApi.Tests.Services.TokenServiceTests;

public abstract class TokenServiceTestsBase
{
	private const string SigningKey = "1234567890123456",
		ValidIssuer = nameof(ValidIssuer);

	private const int AccessExpiration = 15,
		RefreshExpiration = 262800;

	protected TokenServiceTestsBase()
	{
		MockConfiguration
			.SetupConfiguration()
			.Returns(new
			{
				Jwt = new
				{
					ValidIssuer,
					AccessToken = new
					{
						SigningKey,
						ExpirationMinutes = AccessExpiration
					},
					RefreshToken = new
					{
						SigningKey,
						ExpirationMinutes = RefreshExpiration
					}
				}
			});
	}

	protected Mock<IConfiguration> MockConfiguration { get; } = new();

	protected Mock<IDateTimeService> MockDateTimeService { get; } = new();

	protected Mock<IAuthService> MockAuthService { get; } = new();

	protected TokenService CreateFixture() =>
		new(
			MockConfiguration.Object,
			MockDateTimeService.Object,
			MockAuthService.Object);

	protected void VerifyNoOtherCalls()
	{
		MockAuthService.VerifyNoOtherCalls();
	}

	protected static void VerifyAccessToken(AuthGenerateTokensResult.TokenData? token, JwtClaims claims) =>
		VerifyToken(token, claims, AccessExpiration);

	protected static void VerifyRefreshToken(AuthGenerateTokensResult.TokenData? token, JwtClaims claims) =>
		VerifyToken(token, claims, RefreshExpiration);

	public static string GenerateToken(in Guid? tokenId, in long? userId, string? validIssuer = null, string? signingKey = null)
	{
		validIssuer ??= ValidIssuer;
		signingKey ??= SigningKey;

		var (timeSpan, securityKey) = new ConfigurationTokenOptions(TimeSpan.FromMinutes(1), new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
		var claims = new List<Claim>();

		if (tokenId.HasValue)
			claims.Add(new Claim(ApiConst.Claims.TokenId, tokenId.Value.ToString()));
		if (userId.HasValue)
			claims.Add(new Claim(ApiConst.Claims.UserId, userId.Value.ToString()));

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.Add(timeSpan),
			Issuer = validIssuer,
			SigningCredentials = credentials
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private static void VerifyToken(AuthGenerateTokensResult.TokenData? token, JwtClaims claims, int minutes)
	{
		if (token == null)
			throw new InvalidOperationException("Should not verify an empty token");

		var (tokenId, userId, expires) = GetTokenData(token.Value);

		claims.TokenId
			.Should()
			.Be(tokenId);

		claims.UserId
			.Should()
			.Be(userId);

		expires
			.Should()
			.BeCloseTo(DateTime.UtcNow.AddMinutes(minutes), TimeSpan.FromSeconds(10));
	}

	private static JwtData GetTokenData(string token)
	{
		var bytes = Encoding.UTF8.GetBytes(SigningKey);
		var signingKey = new SymmetricSecurityKey(bytes);

		var tokenHandler = new JwtSecurityTokenHandler();
		var parameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			ValidateIssuer = true,
			RequireExpirationTime = true,
			ValidateAudience = false,
			ValidIssuer = ValidIssuer,
			IssuerSigningKey = signingKey,
			ClockSkew = TimeSpan.Zero
		};

		var principal = tokenHandler.ValidateToken(token, parameters, out _);
		return GetJwtData(principal);
	}

	private static JwtData GetJwtData(ClaimsPrincipal principal)
	{
		Guid? tokenId = null;
		long? userId = null;
		DateTime? expires = null;

		foreach (var claim in principal.Claims)
		{
			switch (claim.Type)
			{
				case ApiConst.Claims.TokenId:
					tokenId = Guid.Parse(claim.Value);
					break;
				case ApiConst.Claims.UserId:
					userId = long.Parse(claim.Value);
					break;
				case "exp":
					expires = DateTimeOffset.FromUnixTimeSeconds(long.Parse(claim.Value)).DateTime;
					break;
			}
		}

		return new JwtData(tokenId!.Value, userId!.Value, expires!.Value);
	}

	protected record JwtClaims(Guid TokenId, long UserId);

	private sealed record JwtData(Guid TokenId, long UserId, DateTime Expires) : JwtClaims(TokenId, UserId);
}