namespace MyNihongo.Migrations.Utils.Extensions;

internal static class MigrationEx
{
	public static void RunUp(this Migration @this, ImmutableArray<IMigrationInternal> migrations)
	{
		for (var i = 0; i < migrations.Length; i++)
			migrations[i].Up(@this);
	}

	public static void RunDown(this Migration @this, ImmutableArray<IMigrationInternal> migrations)
	{
		for (var i = migrations.Length - 1; i >= 0; i--)
			migrations[i].Down(@this);
	}

	public static void InvokeAll(this Migration @this, IReadOnlyList<Action<Migration>> actions)
	{
		for (var i = 0; i < actions.Count; i++)
			actions[i](@this);
	}

	public static void DropAll(this Migration @this, IReadOnlyList<string> tables)
	{
		for (var i = 0; i < tables.Count; i++)
			@this.Delete.Table(tables[i]);
	}
}