using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MyNihongo.Migrations.Services;
using MyNihongo.Migrations.Utils;

using var services = new ServiceCollection()
	.AddMigrator("Server=DAYTON033\\SQLEXPRESS;Database=mynihongo;Integrated Security=true;")
	.AddLogging(static x => x.AddFluentMigratorConsole())
	.BuildServiceProvider(true);

using var scope = services.CreateScope();

scope.ServiceProvider.GetRequiredService<IMigrationService>()
	.Migrate();
