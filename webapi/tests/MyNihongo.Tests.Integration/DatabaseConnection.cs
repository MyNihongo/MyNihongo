using MyNihongo.Tests.Integration.Models.Core;
using MyNihongo.Tests.Integration.Models.Kanji;

namespace MyNihongo.Tests.Integration;

public sealed class DatabaseConnection : DataConnection
{
	public DatabaseConnection(string connectionString)
		: base("Microsoft.Data.SqlClient", connectionString)
	{
	}

	#region Core

	public ITable<CoreUserDatabaseRecord> Users => GetTable<CoreUserDatabaseRecord>();

	#endregion

	#region Kanji

	public ITable<KanjiMasterDataDatabaseRecord> KanjiMasterData => GetTable<KanjiMasterDataDatabaseRecord>();

	public ITable<KanjiMeaningDatabaseRecord> KanjiMeanings => GetTable<KanjiMeaningDatabaseRecord>();

	public ITable<KanjiReadingDatabaseRecord> KanjiReading => GetTable<KanjiReadingDatabaseRecord>();

	public ITable<KanjiUserEntryDatabaseRecord> KanjiUserEntries => GetTable<KanjiUserEntryDatabaseRecord>();

	#endregion
}