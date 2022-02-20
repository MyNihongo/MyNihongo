using MyNihongo.WebApi.Resources.Const;
using MyNihongo.WebApi.Services;

namespace MyNihongo.WebApi.Utils.ServiceRegistration;

public static class WebApplicationEx
{
	public static WebApplication UseWebApi(this WebApplication @this)
	{
		@this.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
		@this.UseCors();

		@this.MapGrpcServices();
		return @this;
	}

	private static void MapGrpcServices(this IEndpointRouteBuilder @this)
	{
		MapGrpcService<KanjiController>(@this);

		static void MapGrpcService<T>(IEndpointRouteBuilder @this)
			where T : class
		{
			@this.MapGrpcService<T>().RequireCors(ApiConst.CorsPolicy);
		}
	}
}