using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyNihongo.WebApi.Infrastructure.ServiceRegistration;
using MyNihongo.WebApi.Utils.ServiceRegistration;

namespace MyNihongo.WebApi.Tests.Integration;

public sealed class WebApiFixture : DatabaseFixture
{
	private readonly IHost? _host;
	private readonly TestServer? _testServer;
	private readonly HttpMessageHandler? _httpMessageHandler;

	public WebApiFixture()
		: base(PrepareData)
	{
		var builder = new HostBuilder()
			.ConfigureServices(services =>
			{
				services.AddSingleton(Configuration);
			})
			.ConfigureWebHostDefaults(webHostBuilder =>
			{
				webHostBuilder
					.UseTestServer()
					.UseStartup<Startup>();
			});

		_host = builder.Start();
		_testServer = _host.GetTestServer();
		_httpMessageHandler = _testServer.CreateHandler();
	}

	public override void Dispose()
	{
		_httpMessageHandler?.Dispose();
		_testServer?.Dispose();
		_host?.Dispose();

		base.Dispose();
	}

	private static void PrepareData(DatabaseConnection connection)
	{

	}

	private sealed class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services
				.AddInfrastructure()
				.AddWebApi();
		}

		public static void Configure(IApplicationBuilder app, IHostEnvironment env)
		{
			app.UseWebApiInternal();
		}
	}
}