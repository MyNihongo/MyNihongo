using Microsoft.AspNetCore.Authentication;
using MyNihongo.WebApi.Auth;
using MyNihongo.WebApi.Interceptors;
using MyNihongo.WebApi.Resources.Const;
using MyNihongo.WebApi.Services;

namespace MyNihongo.WebApi.Utils.ServiceRegistration;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddWebApi(this IServiceCollection @this)
	{
		@this.AddGrpc(static x =>
		{
			x.Interceptors.Add<JwtBearerInterceptor>();
		});

		return @this
			.AddCors()
			.AddAuthentication()
			.AddCustomServices();
	}

	private static IServiceCollection AddCors(this IServiceCollection @this)
	{
		@this.AddCors(static opt =>
		{
			opt.AddPolicy(ApiConst.CorsPolicy, static x =>
			{
				x.WithOrigins("http://localhost:8080")
					.WithHeaders("x-grpc-web", "x-user-agent", "content-type", ApiConst.Headers.ClientInfo)
					.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding")
					.AllowCredentials();
			});
		});

		return @this;
	}

	private static IServiceCollection AddAuthentication(this IServiceCollection @this)
	{
		@this
			.AddAuthorization()
			.AddAuthentication(static x =>
			{
				x.DefaultAuthenticateScheme = ApiConst.AuthPolicy;
				x.DefaultChallengeScheme = ApiConst.AuthPolicy;
			}).AddScheme<AuthenticationSchemeOptions, AuthHandler>(ApiConst.AuthPolicy, null);

		return @this
			.AddScoped<UserSession>()
			.AddScoped<IUserSession>(GetUserSession)
			.AddScoped<IMutableUserSession>(GetUserSession)
			.AddSingleton<ITokenService, TokenService>();

		static UserSession GetUserSession(IServiceProvider x) =>
			x.GetRequiredService<UserSession>();
	}

	private static IServiceCollection AddCustomServices(this IServiceCollection @this) =>
		@this
			.AddSingleton<IDateTimeService, DateTimeService>()
			.AddSingleton<IEncryptionService, EncryptionService>();
}