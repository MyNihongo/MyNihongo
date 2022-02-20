namespace MyNihongo.SrcGen.Infrastructure.Database.Tests.DatabaseGeneratorTests.KanjiTests;

public sealed class KanjiGeneratorShould : DatabaseGeneratorTestsBase
{
	[Fact]
	public async Task GenerateKanjiService()
	{
		const string expected = TestUsings +
@"namespace MyNihongo.WebApi.Infrastructure.Kanji;

internal interface IKanjiDatabaseService
{
	/// <summary>
	/// Executes `spKanjiQueryByChar` with the specified <see cref=""parameters""/>
	/// </summary>
	IAsyncEnumerable<KanjiGetListDatabaseRecord> QueryByCharAsync(KanjiGetListByCharParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiQueryByJlpt` with the specified <see cref=""parameters""/>
	/// </summary>
	IAsyncEnumerable<KanjiGetListDatabaseRecord> QueryByJlptAsync(KanjiGetListByJlptParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiQueryByText` with the specified <see cref=""parameters""/>
	/// </summary>
	IAsyncEnumerable<KanjiGetListDatabaseRecord> QueryByTextAsync(KanjiGetListByTextParams parameters, CancellationToken ct = default);
}

internal sealed class KanjiDatabaseService : IKanjiDatabaseService
{
	private readonly IConfiguration _configuration;

	public KanjiDatabaseService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async IAsyncEnumerable<KanjiGetListDatabaseRecord> QueryByCharAsync(KanjiGetListByCharParams parameters, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		if (parameters.Characters.Count > 0)
		{
			await using var tempTableCommand = new SqlCommand(""CREATE TABLE #tmpChar ([char] NCHAR(1) COLLATE Japanese_Bushu_Kakusu_100_BIN)"", connection);
			await tempTableCommand.ExecuteNonQueryAsync(ct).ConfigureAwait(false);

			tempTableCommand.CommandText = ""INSERT #tmpChar ([char]) Values (@Self)"";
			tempTableCommand.Parameters.Add(""Self"", SqlDbType.NChar, 1);

			for (var i = 0; i < parameters.Characters.Count; i++)
			{
				tempTableCommand.Parameters[0].Value = parameters.Characters[i];
				await tempTableCommand.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
			}
		}

		await using var command = new SqlCommand(""spKanjiQueryByChar"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""jlptLevel"", parameters.JlptLevel.HasValue ? parameters.JlptLevel.Value : DBNull.Value);
		command.Parameters.AddWithValue(""filterID"", parameters.Filter);
		command.Parameters.AddWithValue(""langID"", parameters.Language);
		command.Parameters.AddWithValue(""userID"", parameters.UserId.HasValue ? parameters.UserId.Value : DBNull.Value);
		command.Parameters.AddWithValue(""pageIndex"", parameters.PageIndex);
		command.Parameters.AddWithValue(""pageSize"", parameters.PageSize);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		var groupingReadings = new LookUp<short, KanjiGetListDatabaseRecord.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListDatabaseRecord.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListDatabaseRecord.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListDatabaseRecord.Meaning
				{
					KanjiId = reader.GetInt16(0),
					Text = reader.GetString(1)
				};
				groupingMeanings.Add(item.KanjiId, item);
			}
		}

		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var kanjiId = reader.GetInt16(0);
				yield return new KanjiGetListDatabaseRecord
				{
					KanjiId = kanjiId,
					SortingOrder = reader.GetInt16(1),
					Character = reader.GetString(2),
					JlptLevel = reader.GetNullableValue<byte>(3),
					FavouriteRating = reader.GetNullableValue<byte>(4),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}
	}

	public async IAsyncEnumerable<KanjiGetListDatabaseRecord> QueryByJlptAsync(KanjiGetListByJlptParams parameters, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand(""spKanjiQueryByJlpt"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""jlptLevel"", parameters.JlptLevel.HasValue ? parameters.JlptLevel.Value : DBNull.Value);
		command.Parameters.AddWithValue(""filterID"", parameters.Filter);
		command.Parameters.AddWithValue(""langID"", parameters.Language);
		command.Parameters.AddWithValue(""userID"", parameters.UserId.HasValue ? parameters.UserId.Value : DBNull.Value);
		command.Parameters.AddWithValue(""pageIndex"", parameters.PageIndex);
		command.Parameters.AddWithValue(""pageSize"", parameters.PageSize);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		var groupingReadings = new LookUp<short, KanjiGetListDatabaseRecord.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListDatabaseRecord.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListDatabaseRecord.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListDatabaseRecord.Meaning
				{
					KanjiId = reader.GetInt16(0),
					Text = reader.GetString(1)
				};
				groupingMeanings.Add(item.KanjiId, item);
			}
		}

		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var kanjiId = reader.GetInt16(0);
				yield return new KanjiGetListDatabaseRecord
				{
					KanjiId = kanjiId,
					SortingOrder = reader.GetInt16(1),
					Character = reader.GetString(2),
					JlptLevel = reader.GetNullableValue<byte>(3),
					FavouriteRating = reader.GetNullableValue<byte>(4),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}
	}

	public async IAsyncEnumerable<KanjiGetListDatabaseRecord> QueryByTextAsync(KanjiGetListByTextParams parameters, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand(""spKanjiQueryByText"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""text"", parameters.Text);
		command.Parameters.AddWithValue(""byRomaji"", parameters.ByRomaji);
		command.Parameters.AddWithValue(""byLanguage"", parameters.ByLanguage);
		command.Parameters.AddWithValue(""jlptLevel"", parameters.JlptLevel.HasValue ? parameters.JlptLevel.Value : DBNull.Value);
		command.Parameters.AddWithValue(""filterID"", parameters.Filter);
		command.Parameters.AddWithValue(""langID"", parameters.Language);
		command.Parameters.AddWithValue(""userID"", parameters.UserId.HasValue ? parameters.UserId.Value : DBNull.Value);
		command.Parameters.AddWithValue(""pageIndex"", parameters.PageIndex);
		command.Parameters.AddWithValue(""pageSize"", parameters.PageSize);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		var groupingReadings = new LookUp<short, KanjiGetListDatabaseRecord.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListDatabaseRecord.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListDatabaseRecord.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListDatabaseRecord.Meaning
				{
					KanjiId = reader.GetInt16(0),
					Text = reader.GetString(1)
				};
				groupingMeanings.Add(item.KanjiId, item);
			}
		}

		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var kanjiId = reader.GetInt16(0);
				yield return new KanjiGetListDatabaseRecord
				{
					KanjiId = kanjiId,
					SortingOrder = reader.GetInt16(1),
					Character = reader.GetString(2),
					JlptLevel = reader.GetNullableValue<byte>(3),
					FavouriteRating = reader.GetNullableValue<byte>(4),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}
	}
}
";

		await VerifyGeneratorAsync(GeneratorKey.Kanji, expected);
	}
}