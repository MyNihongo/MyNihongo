using MyNihongo.WebApi.Infrastructure.Utils.ServiceRegistration;
using MyNihongo.WebApi.Utils.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

builder.Services
	.AddInfrastructure()
	.AddWebApi();

// Configure app
var app = builder.Build()
	.UseWebApi();

app.Run();
