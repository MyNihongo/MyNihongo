using MyNihongo.WebApi.Resources.Const;

namespace MyNihongo.WebApi.Utils.ServiceRegistration;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddWebApi(this IServiceCollection @this)
	{
		@this.AddGrpc();
		return @this.AddCors();
	}

	private static IServiceCollection AddCors(this IServiceCollection @this)
	{
		@this.AddCors(static opt =>
		{
			opt.AddPolicy(ApiConst.CorsPolicy, static x =>
			{
				x.WithOrigins("http://localhost:8080")
					.WithHeaders("x-grpc-web", "x-user-agent", "content-type")
					.WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
			});
		});

		return @this;
	}
}