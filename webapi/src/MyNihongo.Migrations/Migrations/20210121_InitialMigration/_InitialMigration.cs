using MyNihongo.Migrations.Utils.Extensions;

namespace MyNihongo.Migrations.Migrations;

[TimestampedMigration(2021, 1, 21, 0, 0)]
public sealed class InitialMigration : Migration
{
	private readonly ImmutableArray<IMigrationInternal> _migrations;

	public InitialMigration()
	{
		_migrations = ImmutableArray.Create<IMigrationInternal>(
			new CoreMigration(),
			new KanjiMigration()
		);
	}

	public override void Up() =>
		this.RunUp(_migrations);

	public override void Down() =>
		this.RunDown(_migrations);
}