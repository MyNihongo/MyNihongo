using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Grpc.Net.Client;
using LinqToDB.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.IdentityModel.Tokens;
using MyNihongo.Tests.Integration.Models.Auth;
using MyNihongo.Tests.Integration.Models.Kanji;
using MyNihongo.Tests.Integration.Services;
using MyNihongo.WebApi.Infrastructure.Kanji;
using MyNihongo.WebApi.Infrastructure.ServiceRegistration;
using MyNihongo.WebApi.Models;
using MyNihongo.WebApi.Utils.Extensions;
using MyNihongo.WebApi.Utils.ServiceRegistration;

namespace MyNihongo.WebApi.Tests.Integration;

public sealed class WebApiFixture : DatabaseFixture
{
	private const string ValidIssuer = nameof(ValidIssuer);
	public static readonly Guid AccessId = Guid.Parse("b7f6d856-2ddf-4859-b5be-d86d43a497c7"),
		RefreshId = Guid.Parse("35e4471f-bb68-4353-bc79-f9a924aaed9c");

	private readonly IHost? _host;
	private readonly TestServer? _testServer;
	private readonly HttpMessageHandler? _httpMessageHandler;

	private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
	private GrpcChannel? _channel;

	public WebApiFixture()
		: base(PrepareData, PrepareConfiguration)
	{
		var builder = new HostBuilder()
			.ConfigureServices(services =>
			{
				services
					.AddSingleton(Configuration)
					.AddSingleton(_loggerFactory);
			})
			.ConfigureWebHostDefaults(webHostBuilder =>
			{
				webHostBuilder
					.UseTestServer()
					.UseStartup<Startup>();
			});

		_host = builder.Start();
		_testServer = _host.GetTestServer();
		_httpMessageHandler = _testServer.CreateHandler();

		_loggerFactory.AddProvider(new DebugLoggerProvider());
	}

	public static string User1AccessToken { get; private set; } = string.Empty;

	public static string User1RefreshToken { get; private set; } = string.Empty;

	public GrpcChannel OpenChannel() =>
		_channel ??= CreateChannel();

	public override void Dispose()
	{
		_httpMessageHandler?.Dispose();
		_testServer?.Dispose();
		_host?.Dispose();

		base.Dispose();
	}

	private GrpcChannel CreateChannel()
	{
		return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
		{
			LoggerFactory = _loggerFactory,
			HttpHandler = _httpMessageHandler
		});
	}

	private static void PrepareConfiguration(IConfigurationMockBuilder builder)
	{
		builder.AppendRootSection("Jwt", new
		{
			ValidIssuer,
			AccessToken = new
			{
				SigningKey = "Ckpmp31UKMPJuJT06KNDtqlF7v8v1XWq",
				ExpirationMinutes = 15
			},
			RefreshToken = new
			{
				SigningKey = "qnttupya8dupy3dqxb47p253w7x5ruyr",
				ExpirationMinutes = 262800
			}
		});
	}

	private static void PrepareData(DatabaseConnection connection, IConfiguration configuration)
	{
		// Auth
		var user1Connection1 = Guid.Parse("1f125fac-c08e-4424-986d-bba0a04b686d");

		var accessOptions = configuration.GetAccessTokenOptions();
		var refreshOptions = configuration.GetRefreshTokenOptions();
		
		User1AccessToken = GenerateToken(AccessId, 1L, accessOptions);
		User1RefreshToken = GenerateToken(RefreshId, 1L, refreshOptions);

		connection.Connections.BulkCopy(new AuthConnectionDatabaseRecord[]
		{
			new() { ConnectionId = user1Connection1, UserId = 1L, IpAddress = "81.198.64.253", ClientInfo = "Samsung Galaxy S8", TicksLatestAccess = 123L }
		});

		connection.ConnectionTokens.BulkCopy(new AuthConnectionTokenDatabaseRecord[]
		{
			new() { TokenId = AccessId, ConnectionId = user1Connection1, TicksValidTo = 1234L },
			new() { TokenId = RefreshId, ConnectionId = user1Connection1, TicksValidTo = 123456L }
		});

		// Kanji
		connection.KanjiMasterData.BulkCopy(new KanjiMasterDataDatabaseRecord[]
		{
			new() { KanjiId = 1, SortingOrder = 1, Character = '飲', JlptLevel = JlptLevel.N5 },
			new() { KanjiId = 2, SortingOrder = 1, Character = '願', JlptLevel = JlptLevel.N3 },
		});

		connection.KanjiReading.BulkCopy(new KanjiReadingDatabaseRecord[]
		{
			new() { KanjiId = 1, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "の", SecondaryText = "む", Romaji = "nomu" },
			new() { KanjiId = 1, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "の", SecondaryText = "み", Romaji = "nomi" },
			new() { KanjiId = 1, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "イン", Romaji = "iso" },
			new() { KanjiId = 1, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "オン", Romaji = "oso" },

			new() { KanjiId = 2, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "ねが", SecondaryText = "う", Romaji = "negau" },
			new() { KanjiId = 2, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "ねがい", Romaji = "neko" },
			new() { KanjiId = 2, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ガン", Romaji = "gan" },
		});

		connection.KanjiMeanings.BulkCopy(new KanjiMeaningDatabaseRecord[]
		{
			new() { KanjiId = 1, SortingOrder = 0, Language = Language.English, Text = "drink" },
			new() { KanjiId = 1, SortingOrder = 1, Language = Language.English, Text = "smoke" },
			new() { KanjiId = 1, SortingOrder = 2, Language = Language.English, Text = "take" },
			new() { KanjiId = 1, SortingOrder = 0, Language = Language.French, Text = "boire" },
			new() { KanjiId = 1, SortingOrder = 1, Language = Language.French, Text = "fumer" },
			new() { KanjiId = 1, SortingOrder = 2, Language = Language.French, Text = "prendre" },
			new() { KanjiId = 1, SortingOrder = 0, Language = Language.Spanish, Text = "beber" },
			new() { KanjiId = 1, SortingOrder = 0, Language = Language.Portuguese, Text = "beber" },
			new() { KanjiId = 1, SortingOrder = 1, Language = Language.Portuguese, Text = "fumar" },
			new() { KanjiId = 1, SortingOrder = 2, Language = Language.Portuguese, Text = "tomar" },

			new() { KanjiId = 2, Language = Language.English, SortingOrder = 0, Text = "petition" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 1, Text = "request" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 2, Text = "vow" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 3, Text = "wish" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 4, Text = "hope" },
			new() { KanjiId = 2, Language = Language.French, SortingOrder = 0, Text = "demande" },
			new() { KanjiId = 2, Language = Language.French, SortingOrder = 1, Text = "prière" },
			new() { KanjiId = 2, Language = Language.French, SortingOrder = 2, Text = "souhait" },
			new() { KanjiId = 2, Language = Language.French, SortingOrder = 3, Text = "voeu" },
			new() { KanjiId = 2, Language = Language.French, SortingOrder = 4, Text = "espoir" },
			new() { KanjiId = 2, Language = Language.Spanish, SortingOrder = 0, Text = "petición" },
			new() { KanjiId = 2, Language = Language.Spanish, SortingOrder = 1, Text = "deseo" },
			new() { KanjiId = 2, Language = Language.Spanish, SortingOrder = 2, Text = "anhelo" },
			new() { KanjiId = 2, Language = Language.Spanish, SortingOrder = 3, Text = "desear" },
			new() { KanjiId = 2, Language = Language.Spanish, SortingOrder = 4, Text = "pedir" },
			new() { KanjiId = 2, Language = Language.Spanish, SortingOrder = 5, Text = "rogar" },
			new() { KanjiId = 2, Language = Language.Portuguese, SortingOrder = 0, Text = "petição" },
			new() { KanjiId = 2, Language = Language.Portuguese, SortingOrder = 1, Text = "solicitação" },
			new() { KanjiId = 2, Language = Language.Portuguese, SortingOrder = 2, Text = "voto" },
			new() { KanjiId = 2, Language = Language.Portuguese, SortingOrder = 3, Text = "desejo" },
			new() { KanjiId = 2, Language = Language.Portuguese, SortingOrder = 4, Text = "esperança" },
		});

		connection.KanjiUserEntries.BulkCopy(new KanjiUserEntryDatabaseRecord[]
		{
			new() { KanjiId = 1, UserId = 1L, FavouriteRating = 25, TicksModified = 12345L }
		});
	}

	private static string GenerateToken(in Guid tokenId, in long userId, in ConfigurationTokenOptions options)
	{
		var credentials = new SigningCredentials(options.SigningKey, SecurityAlgorithms.HmacSha256Signature);

		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ApiConst.Claims.TokenId, tokenId.ToString()),
				new(ApiConst.Claims.UserId, userId.ToString())
			}),
			Expires = DateTime.UtcNow.Add(options.Expires),
			Issuer = ValidIssuer,
			SigningCredentials = credentials
		};

		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}

	private sealed class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services
				.AddInfrastructure()
				.AddWebApi();
		}

		public static void Configure(IApplicationBuilder app, IHostEnvironment _)
		{
			app.UseWebApiInternal();
		}
	}
}