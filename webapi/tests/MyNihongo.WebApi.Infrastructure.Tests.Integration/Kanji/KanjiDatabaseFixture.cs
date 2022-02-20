using LinqToDB.Data;
using MyNihongo.Tests.Integration.Models.Kanji;

namespace MyNihongo.WebApi.Infrastructure.Tests.Integration.Kanji;

public sealed class KanjiDatabaseFixture : DatabaseCollectionFixture
{
	public KanjiDatabaseFixture()
		: base(PrepareData)
	{
	}

	private static void PrepareData(DatabaseConnection connection)
	{
		connection.KanjiMasterData.BulkCopy(new KanjiMasterDataDatabaseRecord[]
		{
			// N5
			new() { KanjiId = 5, SortingOrder = 1, Character = '飲', JlptLevel = JlptLevel.N5 },
			new() { KanjiId = 6, SortingOrder = 0, Character = '見', JlptLevel = JlptLevel.N5 },
			new() { KanjiId = 7, SortingOrder = 3, Character = '休', JlptLevel = JlptLevel.N5 },
			new() { KanjiId = 8, SortingOrder = 2, Character = '八', JlptLevel = JlptLevel.N5 },

			// N4
			new() { KanjiId = 9, SortingOrder = 3, Character = '元', JlptLevel = JlptLevel.N4 },
			new() { KanjiId = 10, SortingOrder = 2, Character = '夏', JlptLevel = JlptLevel.N4 },
			new() { KanjiId = 11, SortingOrder = 0, Character = '説', JlptLevel = JlptLevel.N4 },
			new() { KanjiId = 12, SortingOrder = 1, Character = '歩', JlptLevel = JlptLevel.N4 },

			// N3
			new() { KanjiId = 13, SortingOrder = 0, Character = '婚', JlptLevel = JlptLevel.N3 },
			new() { KanjiId = 14, SortingOrder = 3, Character = '猫', JlptLevel = JlptLevel.N3 },
			new() { KanjiId = 15, SortingOrder = 1, Character = '願', JlptLevel = JlptLevel.N3 },
			new() { KanjiId = 16, SortingOrder = 2, Character = '捕', JlptLevel = JlptLevel.N3 },

			// N2
			new() { KanjiId = 17, SortingOrder = 1, Character = '針', JlptLevel = JlptLevel.N2 },
			new() { KanjiId = 18, SortingOrder = 0, Character = '個', JlptLevel = JlptLevel.N2 },
			new() { KanjiId = 19, SortingOrder = 2, Character = '枚', JlptLevel = JlptLevel.N2 },
			new() { KanjiId = 20, SortingOrder = 3, Character = '雲', JlptLevel = JlptLevel.N2 },

			// N1
			new() { KanjiId = 21, SortingOrder = 2, Character = '爵', JlptLevel = JlptLevel.N1 },
			new() { KanjiId = 22, SortingOrder = 1, Character = '餓', JlptLevel = JlptLevel.N1 },
			new() { KanjiId = 23, SortingOrder = 3, Character = '癖', JlptLevel = JlptLevel.N1 },
			new() { KanjiId = 24, SortingOrder = 0, Character = '笙', JlptLevel = JlptLevel.N1 },

			// NULL
			new() { KanjiId = 1, SortingOrder = 2, Character = '亜' },
			new() { KanjiId = 2, SortingOrder = 0, Character = '劉' },
			new() { KanjiId = 3, SortingOrder = 3, Character = '黴' },
			new() { KanjiId = 4, SortingOrder = 1, Character = '曝' },
		});

		connection.KanjiReading.BulkCopy(new KanjiReadingDatabaseRecord[]
		{
			#region N5

			// 5
			new() { KanjiId = 5, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "の", SecondaryText = "む", Romaji = "nomu" },
			new() { KanjiId = 5, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "の", SecondaryText = "み", Romaji = "nomi" },
			new() { KanjiId = 5, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "イン", Romaji = "iso" },
			new() { KanjiId = 5, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "オン", Romaji = "oso" },
			// 6
			new() { KanjiId = 6, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "み", SecondaryText = "る", Romaji = "miru" },
			new() { KanjiId = 6, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "み", SecondaryText = "える", Romaji = "mieru" },
			new() { KanjiId = 6, ReadingType = KanjiReadingType.Kun, SortingOrder = 2, MainText = "み", SecondaryText = "せる", Romaji = "miseru" },
			new() { KanjiId = 6, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ケン", Romaji = "ken" },
			// 7
			new() { KanjiId = 7, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "やす", SecondaryText = "む", Romaji = "yasumu" },
			new() { KanjiId = 7, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "やす", SecondaryText = "まる", Romaji = "yasumaru" },
			new() { KanjiId = 7, ReadingType = KanjiReadingType.Kun, SortingOrder = 2, MainText = "やす", SecondaryText = "める", Romaji = "yasumeru" },
			new() { KanjiId = 7, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "キュウ", Romaji = "kyuu" },
			// 8
			new() { KanjiId = 8, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "や", Romaji = "ya" },
			new() { KanjiId = 8, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "や", SecondaryText = "つ", Romaji = "yatsu" },
			new() { KanjiId = 8, ReadingType = KanjiReadingType.Kun, SortingOrder = 2, MainText = "やっ", SecondaryText = "つ", Romaji = "yattsu" },
			new() { KanjiId = 8, ReadingType = KanjiReadingType.Kun, SortingOrder = 3, MainText = "よう", Romaji = "you" },
			new() { KanjiId = 8, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ハチ", Romaji = "hachi" },
			new() { KanjiId = 8, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ハツ", Romaji = "hatsu" },
			#endregion

			#region N4

			// 9
			new() { KanjiId = 9, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "もと", Romaji = "moto" },
			new() { KanjiId = 9, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ゲン", Romaji = "gen" },
			new() { KanjiId = 9, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ガン", Romaji = "gan" },
			// 10
			new() { KanjiId = 10, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "なつ", Romaji = "natsu" },
			new() { KanjiId = 10, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "カ", Romaji = "ka" },
			new() { KanjiId = 10, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ガ", Romaji = "ga" },
			new() { KanjiId = 10, ReadingType = KanjiReadingType.On, SortingOrder = 2, MainText = "ゲ", Romaji = "ge" },
			// 11
			new() { KanjiId = 11, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "と", SecondaryText = "く", Romaji = "toku" },
			new() { KanjiId = 11, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "セツ", Romaji = "setsu" },
			new() { KanjiId = 11, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ゼイ", Romaji = "zei" },
			// 12
			new() { KanjiId = 12, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "ある", SecondaryText = "く", Romaji = "aruku" },
			new() { KanjiId = 12, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "あゆ", SecondaryText = "む", Romaji = "ayumu" },
			new() { KanjiId = 12, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ホ", Romaji = "ho" },
			new() { KanjiId = 12, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ブ", Romaji = "bu" },
			new() { KanjiId = 12, ReadingType = KanjiReadingType.On, SortingOrder = 2, MainText = "フ", Romaji = "fu" },

			#endregion

			#region N3

			// 13
			new() { KanjiId = 13, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "コン", Romaji = "kon" },
			// 14
			new() { KanjiId = 14, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "ねこ", Romaji = "neko" },
			new() { KanjiId = 14, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ビョウ", Romaji = "byou" },
			// 15
			new() { KanjiId = 15, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "ねが", SecondaryText = "う", Romaji = "negau" },
			new() { KanjiId = 15, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "ねがい", Romaji = "neko" },
			new() { KanjiId = 15, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ガン", Romaji = "gan" },
			// 16
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "と", SecondaryText = "らえる", Romaji = "toraeru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "と", SecondaryText = "らわれる", Romaji = "torawareru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 2, MainText = "と", SecondaryText = "る", Romaji = "toru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 3, MainText = "とら", SecondaryText = "える", Romaji = "toraeru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 4, MainText = "とら", SecondaryText = "われる", Romaji = "torawareru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 5, MainText = "つか", SecondaryText = "まえる", Romaji = "tsukamaeru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.Kun, SortingOrder = 6, MainText = "つか", SecondaryText = "まる", Romaji = "tsukamaru" },
			new() { KanjiId = 16, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ホ", Romaji = "ho" },

			#endregion

			#region N2

			// 17
			new() { KanjiId = 17, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "はり", Romaji = "hari" },
			new() { KanjiId = 17, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "シン", Romaji = "shin" },
			// 18
			new() { KanjiId = 18, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "コ", Romaji = "ko" },
			new() { KanjiId = 18, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "カ", Romaji = "ka" },
			// 19
			new() { KanjiId = 19, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "マイ", Romaji = "mai" },
			new() { KanjiId = 19, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "バイ", Romaji = "bai" },
			// 20
			new() { KanjiId = 20, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "くも", Romaji = "kumo" },
			new() { KanjiId = 20, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "ぐも", Romaji = "gumo" },
			new() { KanjiId = 20, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ウン", Romaji = "un" },

			#endregion

			#region N1

			// 21
			new() { KanjiId = 21, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "シャク", Romaji = "shaku" },
			// 22
			new() { KanjiId = 22, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "う", SecondaryText = "える", Romaji = "ueru" },
			new() { KanjiId = 22, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ガ", Romaji = "ga" },
			// 23
			new() { KanjiId = 23, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "くせ", Romaji = "kuse" },
			new() { KanjiId = 23, ReadingType = KanjiReadingType.Kun, SortingOrder = 1, MainText = "くせ", SecondaryText = "に", Romaji = "kuseni" },
			new() { KanjiId = 23, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ヘキ", Romaji = "heki" },
			// 24
			new() { KanjiId = 24, ReadingType = KanjiReadingType.Kun, SortingOrder = 0, MainText = "ふえ", Romaji = "fue" },
			new() { KanjiId = 24, ReadingType = KanjiReadingType.On, SortingOrder = 0, MainText = "ショウ", Romaji = "shou" },
			new() { KanjiId = 24, ReadingType = KanjiReadingType.On, SortingOrder = 1, MainText = "ソウ", Romaji = "sou" },

			#endregion

			#region NULL

			// 1
			new() { KanjiId = 1, SortingOrder = 0, ReadingType = KanjiReadingType.Kun, MainText = "つ", SecondaryText = "ぐ", Romaji = "tsugu" },
			new() { KanjiId = 1, SortingOrder = 0, ReadingType = KanjiReadingType.On, MainText = "ア", Romaji = "a" },

			// 2
			new() { KanjiId = 2, SortingOrder = 0, ReadingType = KanjiReadingType.Kun, MainText = "ころ", SecondaryText = "す", Romaji = "korosu" },
			new() { KanjiId = 2, SortingOrder = 0, ReadingType = KanjiReadingType.On, MainText = "リュウ", Romaji = "ryuu" },
			new() { KanjiId = 2, SortingOrder = 1, ReadingType = KanjiReadingType.On, MainText = "ル", Romaji = "ri" },

			// 3
			new() { KanjiId = 3, SortingOrder = 0, ReadingType = KanjiReadingType.Kun, MainText = "かび", Romaji = "kabi" },
			new() { KanjiId = 3, SortingOrder = 1, ReadingType = KanjiReadingType.Kun, MainText = "かび", SecondaryText = "る", Romaji = "kabiru" },
			new() { KanjiId = 3, SortingOrder = 2, ReadingType = KanjiReadingType.Kun, MainText = "か", SecondaryText = "びる", Romaji = "kabiru" },
			new() { KanjiId = 3, SortingOrder = 0, ReadingType = KanjiReadingType.On, MainText = "バイ", Romaji = "bai" },
			new() { KanjiId = 3, SortingOrder = 1, ReadingType = KanjiReadingType.On, MainText = "ビ", Romaji = "bi" },
			new() { KanjiId = 3, SortingOrder = 2, ReadingType = KanjiReadingType.On, MainText = "マイ", Romaji = "mai" },
			new() { KanjiId = 3, SortingOrder = 3, ReadingType = KanjiReadingType.On, MainText = "ミ", Romaji = "mi" },

			// 4
			new() { KanjiId = 4, SortingOrder = 0, ReadingType = KanjiReadingType.Kun, MainText = "さら", SecondaryText = "す", Romaji = "sarasu" },
			new() { KanjiId = 4, SortingOrder = 1, ReadingType = KanjiReadingType.On, MainText = "バク", Romaji = "baku" },
			new() { KanjiId = 4, SortingOrder = 2, ReadingType = KanjiReadingType.On, MainText = "ホク", Romaji = "poku" },
			new() { KanjiId = 4, SortingOrder = 3, ReadingType = KanjiReadingType.On, MainText = "ボク", Romaji = "boku" },

			#endregion
		});

		connection.KanjiMeanings.BulkCopy(new KanjiMeaningDatabaseRecord[]
		{
			#region N5

			// 5
			new() { KanjiId = 5, SortingOrder = 0, Language = Language.English, Text = "drink" },
			new() { KanjiId = 5, SortingOrder = 1, Language = Language.English, Text = "smoke" },
			new() { KanjiId = 5, SortingOrder = 2, Language = Language.English, Text = "take" },
			new() { KanjiId = 5, SortingOrder = 0, Language = Language.French, Text = "boire" },
			new() { KanjiId = 5, SortingOrder = 1, Language = Language.French, Text = "fumer" },
			new() { KanjiId = 5, SortingOrder = 2, Language = Language.French, Text = "prendre" },
			new() { KanjiId = 5, SortingOrder = 0, Language = Language.Spanish, Text = "beber" },
			new() { KanjiId = 5, SortingOrder = 0, Language = Language.Portuguese, Text = "beber" },
			new() { KanjiId = 5, SortingOrder = 1, Language = Language.Portuguese, Text = "fumar" },
			new() { KanjiId = 5, SortingOrder = 2, Language = Language.Portuguese, Text = "tomar" },
			// 6
			new() { KanjiId = 6, SortingOrder = 0, Language = Language.English, Text = "see" },
			new() { KanjiId = 6, SortingOrder = 1, Language = Language.English, Text = "hopes" },
			new() { KanjiId = 6, SortingOrder = 2, Language = Language.English, Text = "chances" },
			new() { KanjiId = 6, SortingOrder = 3, Language = Language.English, Text = "idea" },
			new() { KanjiId = 6, SortingOrder = 4, Language = Language.English, Text = "opinion" },
			new() { KanjiId = 6, SortingOrder = 5, Language = Language.English, Text = "look at" },
			new() { KanjiId = 6, SortingOrder = 0, Language = Language.French, Text = "regarder" },
			new() { KanjiId = 6, SortingOrder = 1, Language = Language.French, Text = "voir" },
			new() { KanjiId = 6, SortingOrder = 2, Language = Language.French, Text = "visible" },
			new() { KanjiId = 6, SortingOrder = 3, Language = Language.French, Text = "espoirs" },
			new() { KanjiId = 6, SortingOrder = 4, Language = Language.French, Text = "chances" },
			new() { KanjiId = 6, SortingOrder = 0, Language = Language.French, Text = "idée" },
			new() { KanjiId = 6, SortingOrder = 0, Language = Language.Spanish, Text = "vista" },
			new() { KanjiId = 6, SortingOrder = 1, Language = Language.Spanish, Text = "ver" },
			new() { KanjiId = 6, SortingOrder = 2, Language = Language.Spanish, Text = "esperanza" },
			new() { KanjiId = 6, SortingOrder = 3, Language = Language.Spanish, Text = "idea" },
			new() { KanjiId = 6, SortingOrder = 4, Language = Language.Spanish, Text = "opinión" },
			new() { KanjiId = 6, SortingOrder = 5, Language = Language.Spanish, Text = "observar" },
			new() { KanjiId = 6, SortingOrder = 0, Language = Language.Portuguese, Text = "ver" },
			new() { KanjiId = 6, SortingOrder = 1, Language = Language.Portuguese, Text = "esperanças" },
			new() { KanjiId = 6, SortingOrder = 2, Language = Language.Portuguese, Text = "oportunidades" },
			new() { KanjiId = 6, SortingOrder = 3, Language = Language.Portuguese, Text = "idéia" },
			// 7
			new() { KanjiId = 7, SortingOrder = 0, Language = Language.English, Text = "rest" },
			new() { KanjiId = 7, SortingOrder = 1, Language = Language.English, Text = "day off" },
			new() { KanjiId = 7, SortingOrder = 2, Language = Language.English, Text = "retire" },
			new() { KanjiId = 7, SortingOrder = 3, Language = Language.English, Text = "sleep" },
			new() { KanjiId = 7, SortingOrder = 0, Language = Language.French, Text = "repos" },
			new() { KanjiId = 7, SortingOrder = 1, Language = Language.French, Text = "jour de repos" },
			new() { KanjiId = 7, SortingOrder = 2, Language = Language.French, Text = "se coucher" },
			new() { KanjiId = 7, SortingOrder = 3, Language = Language.French, Text = "dormir" },
			new() { KanjiId = 7, SortingOrder = 0, Language = Language.Spanish, Text = "descanso" },
			new() { KanjiId = 7, SortingOrder = 1, Language = Language.Spanish, Text = "descansar" },
			new() { KanjiId = 7, SortingOrder = 2, Language = Language.Spanish, Text = "tomarse un descanso" },
			new() { KanjiId = 7, SortingOrder = 3, Language = Language.Spanish, Text = "estar descansado" },
			new() { KanjiId = 7, SortingOrder = 0, Language = Language.Portuguese, Text = "descanso" },
			new() { KanjiId = 7, SortingOrder = 1, Language = Language.Portuguese, Text = "dia desligado" },
			new() { KanjiId = 7, SortingOrder = 2, Language = Language.Portuguese, Text = "aposentar" },
			new() { KanjiId = 7, SortingOrder = 3, Language = Language.Portuguese, Text = "sono" },
			// 8
			new() { KanjiId = 8, SortingOrder = 0, Language = Language.English, Text = "eight" },
			new() { KanjiId = 8, SortingOrder = 1, Language = Language.English, Text = "eight radical (no. 12)" },
			new() { KanjiId = 8, SortingOrder = 0, Language = Language.French, Text = "huit" },
			new() { KanjiId = 8, SortingOrder = 1, Language = Language.French, Text = "eight huit (no. 12)" },
			new() { KanjiId = 8, SortingOrder = 0, Language = Language.Spanish, Text = "ocho" },
			new() { KanjiId = 8, SortingOrder = 1, Language = Language.Spanish, Text = "8" },
			new() { KanjiId = 8, SortingOrder = 0, Language = Language.Portuguese, Text = "Oito" },
			#endregion

			#region N4

			// 9
			new() { KanjiId = 9, Language = Language.English, SortingOrder = 0, Text = "beginning" },
			new() { KanjiId = 9, Language = Language.English, SortingOrder = 1, Text = "former time" },
			new() { KanjiId = 9, Language = Language.English, SortingOrder = 2, Text = "origin" },
			new() { KanjiId = 9, Language = Language.French, SortingOrder = 0, Text = "commencement" },
			new() { KanjiId = 9, Language = Language.French, SortingOrder = 1, Text = "origine" },
			new() { KanjiId = 9, Language = Language.French, SortingOrder = 2, Text = "ancien temps" },
			new() { KanjiId = 9, Language = Language.Spanish, SortingOrder = 0, Text = "comienzo" },
			new() { KanjiId = 9, Language = Language.Spanish, SortingOrder = 1, Text = "principio" },
			new() { KanjiId = 9, Language = Language.Spanish, SortingOrder = 2, Text = "origen" },
			new() { KanjiId = 9, Language = Language.Spanish, SortingOrder = 3, Text = "jefe" },
			new() { KanjiId = 9, Language = Language.Portuguese, SortingOrder = 0, Text = "começo" },
			new() { KanjiId = 9, Language = Language.Portuguese, SortingOrder = 1, Text = "tempo anterior" },
			new() { KanjiId = 9, Language = Language.Portuguese, SortingOrder = 2, Text = "origem" },
			// 10
			new() { KanjiId = 10, Language = Language.English, SortingOrder = 0, Text = "summer" },
			new() { KanjiId = 10, Language = Language.French, SortingOrder = 0, Text = "été" },
			new() { KanjiId = 10, Language = Language.Spanish, SortingOrder = 0, Text = "verano" },
			new() { KanjiId = 10, Language = Language.Portuguese, SortingOrder = 0, Text = "verão" },
			// 11
			new() { KanjiId = 11, Language = Language.English, SortingOrder = 0, Text = "opinion" },
			new() { KanjiId = 11, Language = Language.English, SortingOrder = 1, Text = "theory" },
			new() { KanjiId = 11, Language = Language.English, SortingOrder = 2, Text = "explanation" },
			new() { KanjiId = 11, Language = Language.English, SortingOrder = 3, Text = "rumor" },
			new() { KanjiId = 11, Language = Language.French, SortingOrder = 0, Text = "théorie" },
			new() { KanjiId = 11, Language = Language.French, SortingOrder = 1, Text = "rumeur" },
			new() { KanjiId = 11, Language = Language.French, SortingOrder = 2, Text = "opinion" },
			new() { KanjiId = 11, Language = Language.French, SortingOrder = 3, Text = "thèse" },
			new() { KanjiId = 11, Language = Language.Spanish, SortingOrder = 0, Text = "opinión" },
			new() { KanjiId = 11, Language = Language.Spanish, SortingOrder = 1, Text = "teoría" },
			new() { KanjiId = 11, Language = Language.Spanish, SortingOrder = 2, Text = "explicar" },
			new() { KanjiId = 11, Language = Language.Spanish, SortingOrder = 3, Text = "expresar" },
			new() { KanjiId = 11, Language = Language.Portuguese, SortingOrder = 0, Text = "rumor" },
			new() { KanjiId = 11, Language = Language.Portuguese, SortingOrder = 1, Text = "opinião" },
			new() { KanjiId = 11, Language = Language.Portuguese, SortingOrder = 2, Text = "teoria" },
			// 12
			new() { KanjiId = 12, Language = Language.English, SortingOrder = 0, Text = "walk" },
			new() { KanjiId = 12, Language = Language.English, SortingOrder = 1, Text = "counter for steps" },
			new() { KanjiId = 12, Language = Language.French, SortingOrder = 0, Text = "marcher" },
			new() { KanjiId = 12, Language = Language.French, SortingOrder = 1, Text = "compteur de pas" },
			new() { KanjiId = 12, Language = Language.Spanish, SortingOrder = 0, Text = "camino" },
			new() { KanjiId = 12, Language = Language.Spanish, SortingOrder = 1, Text = "caminar" },
			new() { KanjiId = 12, Language = Language.Spanish, SortingOrder = 2, Text = "razón" },
			new() { KanjiId = 12, Language = Language.Spanish, SortingOrder = 3, Text = "proporción" },
			new() { KanjiId = 12, Language = Language.Spanish, SortingOrder = 4, Text = "ir a pie" },
			new() { KanjiId = 12, Language = Language.Portuguese, SortingOrder = 0, Text = "passeio" },

			#endregion

			#region N3

			// 13
			new() { KanjiId = 13, Language = Language.English, SortingOrder = 0, Text = "marriage" },
			new() { KanjiId = 13, Language = Language.French, SortingOrder = 0, Text = "mariage" },
			new() { KanjiId = 13, Language = Language.Spanish, SortingOrder = 0, Text = "matrimonio" },
			new() { KanjiId = 13, Language = Language.Spanish, SortingOrder = 1, Text = "casamiento" },
			new() { KanjiId = 13, Language = Language.Portuguese, SortingOrder = 0, Text = "casamento" },
			// 14
			new() { KanjiId = 14, Language = Language.English, SortingOrder = 0, Text = "cat" },
			new() { KanjiId = 14, Language = Language.French, SortingOrder = 0, Text = "chat" },
			new() { KanjiId = 14, Language = Language.Spanish, SortingOrder = 0, Text = "gato" },
			new() { KanjiId = 14, Language = Language.Portuguese, SortingOrder = 0, Text = "Gato" },
			// 15
			new() { KanjiId = 15, Language = Language.English, SortingOrder = 0, Text = "petition" },
			new() { KanjiId = 15, Language = Language.English, SortingOrder = 1, Text = "request" },
			new() { KanjiId = 15, Language = Language.English, SortingOrder = 2, Text = "vow" },
			new() { KanjiId = 15, Language = Language.English, SortingOrder = 3, Text = "wish" },
			new() { KanjiId = 15, Language = Language.English, SortingOrder = 4, Text = "hope" },
			new() { KanjiId = 15, Language = Language.French, SortingOrder = 0, Text = "demande" },
			new() { KanjiId = 15, Language = Language.French, SortingOrder = 1, Text = "prière" },
			new() { KanjiId = 15, Language = Language.French, SortingOrder = 2, Text = "souhait" },
			new() { KanjiId = 15, Language = Language.French, SortingOrder = 3, Text = "voeu" },
			new() { KanjiId = 15, Language = Language.French, SortingOrder = 4, Text = "espoir" },
			new() { KanjiId = 15, Language = Language.Spanish, SortingOrder = 0, Text = "petición" },
			new() { KanjiId = 15, Language = Language.Spanish, SortingOrder = 1, Text = "deseo" },
			new() { KanjiId = 15, Language = Language.Spanish, SortingOrder = 2, Text = "anhelo" },
			new() { KanjiId = 15, Language = Language.Spanish, SortingOrder = 3, Text = "desear" },
			new() { KanjiId = 15, Language = Language.Spanish, SortingOrder = 4, Text = "pedir" },
			new() { KanjiId = 15, Language = Language.Spanish, SortingOrder = 5, Text = "rogar" },
			new() { KanjiId = 15, Language = Language.Portuguese, SortingOrder = 0, Text = "petição" },
			new() { KanjiId = 15, Language = Language.Portuguese, SortingOrder = 1, Text = "solicitação" },
			new() { KanjiId = 15, Language = Language.Portuguese, SortingOrder = 2, Text = "voto" },
			new() { KanjiId = 15, Language = Language.Portuguese, SortingOrder = 3, Text = "desejo" },
			new() { KanjiId = 15, Language = Language.Portuguese, SortingOrder = 4, Text = "esperança" },
			// 16
			new() { KanjiId = 16, Language = Language.English, SortingOrder = 0, Text = "catch" },
			new() { KanjiId = 16, Language = Language.English, SortingOrder = 1, Text = "capture" },
			new() { KanjiId = 16, Language = Language.French, SortingOrder = 0, Text = "attraper" },
			new() { KanjiId = 16, Language = Language.French, SortingOrder = 1, Text = "capturer" },
			new() { KanjiId = 16, Language = Language.Spanish, SortingOrder = 0, Text = "atrapar" },
			new() { KanjiId = 16, Language = Language.Spanish, SortingOrder = 1, Text = "capturar" },
			new() { KanjiId = 16, Language = Language.Spanish, SortingOrder = 2, Text = "coger" },
			new() { KanjiId = 16, Language = Language.Spanish, SortingOrder = 3, Text = "ser atrapado" },
			new() { KanjiId = 16, Language = Language.Spanish, SortingOrder = 4, Text = "ser capturado" },
			new() { KanjiId = 16, Language = Language.Portuguese, SortingOrder = 0, Text = "pegar" },
			new() { KanjiId = 16, Language = Language.Portuguese, SortingOrder = 1, Text = "capturar" },

			#endregion

			#region N2

			// 17
			new() { KanjiId = 17, Language = Language.English, SortingOrder = 0, Text = "needle" },
			new() { KanjiId = 17, Language = Language.English, SortingOrder = 1, Text = "pin" },
			new() { KanjiId = 17, Language = Language.English, SortingOrder = 2, Text = "staple" },
			new() { KanjiId = 17, Language = Language.English, SortingOrder = 3, Text = "stinger" },
			new() { KanjiId = 17, Language = Language.French, SortingOrder = 0, Text = "aiguille" },
			new() { KanjiId = 17, Language = Language.French, SortingOrder = 1, Text = "épingle" },
			new() { KanjiId = 17, Language = Language.French, SortingOrder = 2, Text = "agrafe" },
			new() { KanjiId = 17, Language = Language.French, SortingOrder = 3, Text = "dard" },
			new() { KanjiId = 17, Language = Language.Spanish, SortingOrder = 0, Text = "aguja" },
			new() { KanjiId = 17, Language = Language.Spanish, SortingOrder = 1, Text = "alfiler" },
			new() { KanjiId = 17, Language = Language.Portuguese, SortingOrder = 0, Text = "agulha" },
			new() { KanjiId = 17, Language = Language.Portuguese, SortingOrder = 1, Text = "alfinete" },
			new() { KanjiId = 17, Language = Language.Portuguese, SortingOrder = 2, Text = "grampo" },
			new() { KanjiId = 17, Language = Language.Portuguese, SortingOrder = 3, Text = "o que pica" },
			// 18
			new() { KanjiId = 18, Language = Language.English, SortingOrder = 0, Text = "individual" },
			new() { KanjiId = 18, Language = Language.English, SortingOrder = 1, Text = "counter for articles" },
			new() { KanjiId = 18, Language = Language.French, SortingOrder = 0, Text = "individuel" },
			new() { KanjiId = 18, Language = Language.French, SortingOrder = 1, Text = "privé" },
			new() { KanjiId = 18, Language = Language.French, SortingOrder = 2, Text = "compteur d'articles" },
			new() { KanjiId = 18, Language = Language.French, SortingOrder = 3, Text = "compteur d'unités militaires" },
			new() { KanjiId = 18, Language = Language.Spanish, SortingOrder = 0, Text = "individuo" },
			new() { KanjiId = 18, Language = Language.Spanish, SortingOrder = 1, Text = "individual" },
			new() { KanjiId = 18, Language = Language.Spanish, SortingOrder = 2, Text = "contador para objetos pequeños" },
			new() { KanjiId = 18, Language = Language.Portuguese, SortingOrder = 0, Text = "indivíduo" },
			new() { KanjiId = 18, Language = Language.Portuguese, SortingOrder = 1, Text = "sufixo para contagem de para artigos" },
			// 19
			new() { KanjiId = 19, Language = Language.English, SortingOrder = 0, Text = "sheet of..." },
			new() { KanjiId = 19, Language = Language.English, SortingOrder = 1, Text = "counter for flat thin objects or sheets" },
			new() { KanjiId = 19, Language = Language.French, SortingOrder = 0, Text = "feuille de" },
			new() { KanjiId = 19, Language = Language.French, SortingOrder = 1, Text = "compteur d'objets plats" },
			new() { KanjiId = 19, Language = Language.Spanish, SortingOrder = 0, Text = "contar" },
			new() { KanjiId = 19, Language = Language.Spanish, SortingOrder = 1, Text = "contador para objetos delgados y planos" },
			new() { KanjiId = 19, Language = Language.Spanish, SortingOrder = 2, Text = "antiguamente dinero" },
			new() { KanjiId = 19, Language = Language.Portuguese, SortingOrder = 0, Text = "folha de..." },
			new() { KanjiId = 19, Language = Language.Portuguese, SortingOrder = 1, Text = "sufixo para contagem de para objetos finos" },
			new() { KanjiId = 19, Language = Language.Portuguese, SortingOrder = 2, Text = "chatos ou folhas" },
			// 20
			new() { KanjiId = 20, Language = Language.English, SortingOrder = 0, Text = "cloud" },
			new() { KanjiId = 20, Language = Language.French, SortingOrder = 0, Text = "nuage" },
			new() { KanjiId = 20, Language = Language.Spanish, SortingOrder = 0, Text = "nube" },
			new() { KanjiId = 20, Language = Language.Portuguese, SortingOrder = 0, Text = "nuvem" },

			#endregion

			#region N1

			// 21
			new() { KanjiId = 21, Language = Language.English, SortingOrder = 0, Text = "baron" },
			new() { KanjiId = 21, Language = Language.English, SortingOrder = 1, Text = "peerage" },
			new() { KanjiId = 21, Language = Language.English, SortingOrder = 2, Text = "court rank" },
			new() { KanjiId = 21, Language = Language.French, SortingOrder = 0, Text = "baron" },
			new() { KanjiId = 21, Language = Language.French, SortingOrder = 1, Text = "pairie" },
			new() { KanjiId = 21, Language = Language.French, SortingOrder = 2, Text = "rang à la cour" },
			new() { KanjiId = 21, Language = Language.Spanish, SortingOrder = 0, Text = "linaje" },
			new() { KanjiId = 21, Language = Language.Spanish, SortingOrder = 1, Text = "título nobiliario" },
			new() { KanjiId = 21, Language = Language.Portuguese, SortingOrder = 0, Text = "barão" },
			new() { KanjiId = 21, Language = Language.Portuguese, SortingOrder = 1, Text = "nobreza" },
			new() { KanjiId = 21, Language = Language.Portuguese, SortingOrder = 2, Text = "posição na nobreza" },
			// 22
			new() { KanjiId = 22, Language = Language.English, SortingOrder = 0, Text = "starve" },
			new() { KanjiId = 22, Language = Language.English, SortingOrder = 1, Text = "hungry" },
			new() { KanjiId = 22, Language = Language.English, SortingOrder = 2, Text = "thirst" },
			new() { KanjiId = 22, Language = Language.French, SortingOrder = 0, Text = "famine" },
			new() { KanjiId = 22, Language = Language.French, SortingOrder = 1, Text = "faim" },
			new() { KanjiId = 22, Language = Language.French, SortingOrder = 2, Text = "soif" },
			new() { KanjiId = 22, Language = Language.Spanish, SortingOrder = 0, Text = "hambre" },
			new() { KanjiId = 22, Language = Language.Spanish, SortingOrder = 1, Text = "estar hambriento" },
			new() { KanjiId = 22, Language = Language.Portuguese, SortingOrder = 0, Text = "Morrer de fome" },
			new() { KanjiId = 22, Language = Language.Portuguese, SortingOrder = 1, Text = "com fome" },
			new() { KanjiId = 22, Language = Language.Portuguese, SortingOrder = 2, Text = "sede" },
			// 23
			new() { KanjiId = 23, Language = Language.English, SortingOrder = 0, Text = "mannerism" },
			new() { KanjiId = 23, Language = Language.English, SortingOrder = 1, Text = "habit" },
			new() { KanjiId = 23, Language = Language.English, SortingOrder = 2, Text = "vice" },
			new() { KanjiId = 23, Language = Language.English, SortingOrder = 3, Text = "trait" },
			new() { KanjiId = 23, Language = Language.English, SortingOrder = 4, Text = "fault" },
			new() { KanjiId = 23, Language = Language.English, SortingOrder = 5, Text = "kink" },
			new() { KanjiId = 23, Language = Language.French, SortingOrder = 0, Text = "manie" },
			new() { KanjiId = 23, Language = Language.French, SortingOrder = 1, Text = "mauvaise habitude" },
			new() { KanjiId = 23, Language = Language.French, SortingOrder = 2, Text = "vice" },
			new() { KanjiId = 23, Language = Language.French, SortingOrder = 3, Text = "trait (caractère)" },
			new() { KanjiId = 23, Language = Language.French, SortingOrder = 4, Text = "défaut" },
			new() { KanjiId = 23, Language = Language.French, SortingOrder = 5, Text = "frisé" },
			new() { KanjiId = 23, Language = Language.Spanish, SortingOrder = 0, Text = "costumbre" },
			new() { KanjiId = 23, Language = Language.Spanish, SortingOrder = 1, Text = "hábito" },
			new() { KanjiId = 23, Language = Language.Spanish, SortingOrder = 2, Text = "debilidad" },
			new() { KanjiId = 23, Language = Language.Spanish, SortingOrder = 3, Text = "vicio" },
			new() { KanjiId = 23, Language = Language.Spanish, SortingOrder = 4, Text = "manía" },
			new() { KanjiId = 23, Language = Language.Portuguese, SortingOrder = 0, Text = "maneirismo" },
			new() { KanjiId = 23, Language = Language.Portuguese, SortingOrder = 1, Text = "hábito" },
			new() { KanjiId = 23, Language = Language.Portuguese, SortingOrder = 2, Text = "vício" },
			new() { KanjiId = 23, Language = Language.Portuguese, SortingOrder = 3, Text = "traço" },
			new() { KanjiId = 23, Language = Language.Portuguese, SortingOrder = 4, Text = "falta" },
			new() { KanjiId = 23, Language = Language.Portuguese, SortingOrder = 5, Text = "entortar" },
			// 24
			new() { KanjiId = 24, Language = Language.English, SortingOrder = 0, Text = "a reed instrument" },
			new() { KanjiId = 24, Language = Language.Spanish, SortingOrder = 0, Text = "instrumento de junco" },
			new() { KanjiId = 24, Language = Language.Spanish, SortingOrder = 1, Text = "instrumento de viento con lengüeta" },

			#endregion

			#region NULL

			// 1
			new() { KanjiId = 1, Language = Language.English, SortingOrder = 0, Text = "Asia" },
			new() { KanjiId = 1, Language = Language.English, SortingOrder = 1, Text = "rank next" },
			new() { KanjiId = 1, Language = Language.English, SortingOrder = 2, Text = "come after" },
			new() { KanjiId = 1, Language = Language.English, SortingOrder = 3, Text = "-ous" },
			new() { KanjiId = 1, Language = Language.French, SortingOrder = 0, Text = "Asie" },
			new() { KanjiId = 1, Language = Language.French, SortingOrder = 1, Text = "suivant" },
			new() { KanjiId = 1, Language = Language.French, SortingOrder = 2, Text = "sub-" },
			new() { KanjiId = 1, Language = Language.French, SortingOrder = 3, Text = "sous-" },
			new() { KanjiId = 1, Language = Language.Spanish, SortingOrder = 0, Text = "pref. para indicar" },
			new() { KanjiId = 1, Language = Language.Spanish, SortingOrder = 1, Text = "venir después de" },
			new() { KanjiId = 1, Language = Language.Spanish, SortingOrder = 2, Text = "Asia" },
			new() { KanjiId = 1, Language = Language.Portuguese, SortingOrder = 0, Text = "Ásia" },
			new() { KanjiId = 1, Language = Language.Portuguese, SortingOrder = 1, Text = "próxima" },
			new() { KanjiId = 1, Language = Language.Portuguese, SortingOrder = 2, Text = "o que vem depois" },

			// 2
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 0, Text = "weapon of war" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 1, Text = "logging axe" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 2, Text = "kill en masse" },
			new() { KanjiId = 2, Language = Language.English, SortingOrder = 3, Text = "peeling (paint off a wall, etc)" },

			// 3
			new() { KanjiId = 3, Language = Language.English, SortingOrder = 0, Text = "mold" },
			new() { KanjiId = 3, Language = Language.English, SortingOrder = 1, Text = "mildew" },

			// 4
			new() { KanjiId = 4, Language = Language.English, SortingOrder = 0, Text = "bleach" },
			new() { KanjiId = 4, Language = Language.English, SortingOrder = 1, Text = "refine" },
			new() { KanjiId = 4, Language = Language.English, SortingOrder = 2, Text = "expose" },
			new() { KanjiId = 4, Language = Language.English, SortingOrder = 3, Text = "air" },

			#endregion
		});

		connection.KanjiUserEntries.BulkCopy(new KanjiUserEntryDatabaseRecord[]
		{
			// 1
			new() { UserId = 1L, KanjiId = 5 },
			new() { UserId = 1L, KanjiId = 7 },
			new() { UserId = 1L, KanjiId = 8, IsDeleted = true, FavouriteRating = 10 },
			new() { UserId = 1L, KanjiId = 11, FavouriteRating = 50 },

			// 2
			new() { UserId = 2L, KanjiId = 5, FavouriteRating = 45 },
			new() { UserId = 2L, KanjiId = 9 },
			new() { UserId = 2L, KanjiId = 7, FavouriteRating = 20 },
			new() { UserId = 2L, KanjiId = 11, FavouriteRating = 20 },
			new() { UserId = 2L, KanjiId = 10, IsDeleted = true, FavouriteRating = 20 },
			new() { UserId = 2L, KanjiId = 15 }
		});
	}
}