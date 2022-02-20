using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MyNihongo.Migrations.Migrations;
using MyNihongo.Migrations.Models.Options;
using MyNihongo.Migrations.Services;

namespace MyNihongo.Migrations.Utils;

public static class ServiceCollectionEx
{
	public static IServiceCollection AddMigrator(this IServiceCollection @this, string connectionString)
	{
		@this.AddFluentMigratorCore()
			.ConfigureRunner(cfg =>
			{
				cfg.AddSqlServer()
					.WithGlobalConnectionString(connectionString)
					.ScanIn(typeof(InitialMigration).Assembly).For.Migrations();
			});

		return @this
			.Configure<ConnectionString>(x => x.Value = connectionString)
			.AddTransient<IScriptRunner, ScriptRunner>()
			.AddTransient<IMigrationService, MigrationService>();
	}
}