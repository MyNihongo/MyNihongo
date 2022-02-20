namespace MyNihongo.WebApi.Infrastructure.Utils.ServiceRegistration;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection @this) =>
		@this
			.AddMediatR(typeof(ServiceCollectionEx).Assembly)
			.AddDatabaseServices();
}