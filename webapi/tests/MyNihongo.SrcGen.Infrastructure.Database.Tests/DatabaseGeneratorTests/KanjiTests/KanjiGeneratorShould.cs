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
	/// Executes `spKanjiQueryDetail` with the specified <see cref=""parameters""/>
	/// </summary>
	Task<KanjiGetDetailResult> QueryDetailAsync(KanjiGetDetailParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiQueryByChar` with the specified <see cref=""parameters""/>
	/// </summary>
	IAsyncEnumerable<KanjiGetListResult> QueryByCharAsync(KanjiGetListByCharParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiQueryByJlpt` with the specified <see cref=""parameters""/>
	/// </summary>
	IAsyncEnumerable<KanjiGetListResult> QueryByJlptAsync(KanjiGetListByJlptParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiQueryByText` with the specified <see cref=""parameters""/>
	/// </summary>
	IAsyncEnumerable<KanjiGetListResult> QueryByTextAsync(KanjiGetListByTextParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiUserAdd` with the specified <see cref=""parameters""/>
	/// </summary>
	Task<KanjiUserAddResult> UserAddAsync(KanjiUserAddParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiUserRemove` with the specified <see cref=""parameters""/>
	/// </summary>
	Task UserRemoveAsync(KanjiUserRemoveParams parameters, CancellationToken ct = default);

	/// <summary>
	/// Executes `spKanjiUserSet` with the specified <see cref=""parameters""/>
	/// </summary>
	Task UserSetAsync(KanjiUserSetParams parameters, CancellationToken ct = default);
}

internal sealed class KanjiDatabaseService : IKanjiDatabaseService
{
	private readonly IConfiguration _configuration;

	public KanjiDatabaseService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public async Task<KanjiGetDetailResult> QueryDetailAsync(KanjiGetDetailParams parameters, CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand(""spKanjiQueryDetail"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""langID"", parameters.Language);
		command.Parameters.AddWithValue(""userID"", parameters.UserId.HasValue ? parameters.UserId.Value : DBNull.Value);
		command.Parameters.AddWithValue(""kanjiID"", parameters.KanjiId);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		var groupingReadings = new LookUp<short, KanjiGetListResult.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListResult.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListResult.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListResult.Meaning
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
				return new KanjiGetDetailResult
				{
					KanjiId = kanjiId,
					Character = reader.GetString(1),
					JlptLevel = reader.GetNullableStruct<byte>(2),
					FavouriteRating = reader.GetNullableStruct<byte>(3),
					Notes = reader.GetNullableRef<string>(4),
					Mark = reader.GetNullableStruct<byte>(5),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}

		return new KanjiGetDetailResult();
	}

	public async IAsyncEnumerable<KanjiGetListResult> QueryByCharAsync(KanjiGetListByCharParams parameters, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
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

		var groupingReadings = new LookUp<short, KanjiGetListResult.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListResult.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListResult.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListResult.Meaning
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
				yield return new KanjiGetListResult
				{
					KanjiId = kanjiId,
					SortingOrder = reader.GetInt16(1),
					Character = reader.GetString(2),
					JlptLevel = reader.GetNullableStruct<byte>(3),
					FavouriteRating = reader.GetNullableStruct<byte>(4),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}
	}

	public async IAsyncEnumerable<KanjiGetListResult> QueryByJlptAsync(KanjiGetListByJlptParams parameters, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
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

		var groupingReadings = new LookUp<short, KanjiGetListResult.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListResult.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListResult.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListResult.Meaning
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
				yield return new KanjiGetListResult
				{
					KanjiId = kanjiId,
					SortingOrder = reader.GetInt16(1),
					Character = reader.GetString(2),
					JlptLevel = reader.GetNullableStruct<byte>(3),
					FavouriteRating = reader.GetNullableStruct<byte>(4),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}
	}

	public async IAsyncEnumerable<KanjiGetListResult> QueryByTextAsync(KanjiGetListByTextParams parameters, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
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

		var groupingReadings = new LookUp<short, KanjiGetListResult.Reading>();
		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			var item = new KanjiGetListResult.Reading
			{
				KanjiId = reader.GetInt16(0),
				ReadingType = reader.GetByte(1),
				MainText = reader.GetString(2),
				SecondaryText = reader.GetString(3),
				RomajiReading = reader.GetString(4)
			};
			groupingReadings.Add(item.KanjiId, item);
		}

		var groupingMeanings = new LookUp<short, KanjiGetListResult.Meaning>();
		if (await reader.NextResultAsync(ct).ConfigureAwait(false))
		{
			while (await reader.ReadAsync(ct).ConfigureAwait(false))
			{
				var item = new KanjiGetListResult.Meaning
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
				yield return new KanjiGetListResult
				{
					KanjiId = kanjiId,
					SortingOrder = reader.GetInt16(1),
					Character = reader.GetString(2),
					JlptLevel = reader.GetNullableStruct<byte>(3),
					FavouriteRating = reader.GetNullableStruct<byte>(4),
					Readings = groupingReadings.GetItems(kanjiId),
					Meanings = groupingMeanings.GetItems(kanjiId)
				};
			}
		}
	}

	public async Task<KanjiUserAddResult> UserAddAsync(KanjiUserAddParams parameters, CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand(""spKanjiUserAdd"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""userID"", parameters.UserId);
		command.Parameters.AddWithValue(""kanjiID"", parameters.KanjiId);
		command.Parameters.AddWithValue(""ticksModified"", parameters.TicksModified);

		await using var reader = await command.ExecuteReaderAsync(ct)
			.ConfigureAwait(false);

		while (await reader.ReadAsync(ct).ConfigureAwait(false))
		{
			return new KanjiUserAddResult
			{
				FavouriteRating = reader.GetByte(0),
				Notes = reader.GetString(1),
				Mark = reader.GetByte(2)
			};
		}

		return new KanjiUserAddResult();
	}

	public async Task UserRemoveAsync(KanjiUserRemoveParams parameters, CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand(""spKanjiUserRemove"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""userID"", parameters.UserId);
		command.Parameters.AddWithValue(""kanjiID"", parameters.KanjiId);
		command.Parameters.AddWithValue(""ticksModified"", parameters.TicksModified);

		await command.ExecuteNonQueryAsync(ct)
			.ConfigureAwait(false);
	}

	public async Task UserSetAsync(KanjiUserSetParams parameters, CancellationToken ct = default)
	{
		await using var connection = new SqlConnection(_configuration.GetConnectionString(ConnectionType.Standard));
		await connection.OpenAsync(ct).ConfigureAwait(false);

		await using var command = new SqlCommand(""spKanjiUserSet"", connection) { CommandType = CommandType.StoredProcedure };
		command.Parameters.AddWithValue(""userID"", parameters.UserId);
		command.Parameters.AddWithValue(""kanjiID"", parameters.KanjiId);
		command.Parameters.AddWithValue(""rating"", parameters.FavouriteRating.HasValue ? parameters.FavouriteRating.Value : DBNull.Value);
		command.Parameters.AddWithValue(""notes"", parameters.Notes != null ? parameters.Notes : DBNull.Value);
		command.Parameters.AddWithValue(""mark"", parameters.Mark.HasValue ? parameters.Mark.Value : DBNull.Value);
		command.Parameters.AddWithValue(""ticksModified"", parameters.TicksModified);

		await command.ExecuteNonQueryAsync(ct)
			.ConfigureAwait(false);
	}
}
";

		await VerifyGeneratorAsync(GeneratorKey.Kanji, expected);
	}
}