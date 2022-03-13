using MyNihongo.WebApi.Resources.Const;

namespace MyNihongo.WebApi.Utils.ServiceRegistration;

public static class WebApplicationEx
{
	public static WebApplication UseWebApi(this WebApplication @this)
	{
		@this.UseWebApiInternal();
		return @this;
	}

	public static void UseWebApiInternal(this IApplicationBuilder @this)
	{
		@this.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

		@this.UseHttpsRedirection();
		@this.UseRouting();

		@this.UseCors();

		@this.UseAuthentication();
		@this.UseAuthorization();

		@this.UseEndpoints(static x =>
		{
			x.MapGrpcServices();
		});
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