using FluentMigrator.Runner;

namespace MyNihongo.Migrations.Services;

internal sealed class MigrationService : IMigrationService
{
	private readonly IMigrationRunner _migrationRunner;
	private readonly IScriptRunner _scriptRunner;

	public MigrationService(
		IMigrationRunner migrationRunner,
		IScriptRunner scriptRunner)
	{
		_migrationRunner = migrationRunner;
		_scriptRunner = scriptRunner;
	}

	public void Migrate()
	{
		_migrationRunner.MigrateUp();
		_scriptRunner.Execute();
	}
}