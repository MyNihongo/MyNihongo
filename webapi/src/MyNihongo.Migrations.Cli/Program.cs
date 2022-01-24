using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MyNihongo.Migrations.Migrations;

using var services = new ServiceCollection()
	.AddFluentMigratorCore()
	.ConfigureRunner(x =>
	{
		x.AddSqlServer2016()
			.WithGlobalConnectionString("Server=DAYTON033;Database=my_nihongo;Integrated Security=true;")
			.ScanIn(typeof(InitialMigration).Assembly)
			.For.Migrations();
	})
	.AddLogging(static x => x.AddFluentMigratorConsole())
	.BuildServiceProvider(true);

using var scope = services.CreateScope();

scope.ServiceProvider.GetRequiredService<IMigrationRunner>()
	.MigrateUp();