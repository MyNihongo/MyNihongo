namespace MyNihongo.Migrations;

internal interface IMigrationInternal
{
	void Up(Migration migration);

	void Down(Migration migration);
}