namespace MyNihongo.WebApi.Infrastructure.ServiceRegistration;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection @this) =>
		@this
			.AddMediatR(typeof(ServiceCollectionEx).Assembly)
			.AddSingleton<IClock>(SystemClock.Instance)
			.AddDatabaseServices();
}