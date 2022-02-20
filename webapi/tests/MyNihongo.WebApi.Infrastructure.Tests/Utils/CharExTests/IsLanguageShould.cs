namespace MyNihongo.WebApi.Infrastructure.Tests.Utils.CharExTests;

public sealed class IsLanguageShould
{
	[Fact]
	public void BeTrueForCommonChars()
	{
		var languages = Enum.GetValues<Language>()
			.Where(x => x != Language.UndefinedLanguage);

		foreach (var language in languages)
		{
			foreach (var c in GetCommonChars())
			{
				var result = c.IsLanguage(language);

				result
					.Should()
					.BeTrue();
			}
		}

		static IEnumerable<char> GetCommonChars()
		{
			for (var c = '0'; c <= '9'; c++)
				yield return c;

			yield return ' ';
			yield return '.';
		}
	}

	[Fact]
	public void BeTrueForEnglish()
	{
		const Language language = Language.English;

		foreach (var c in GetEnglishChars())
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeTrue();
		}
	}

	[Fact]
	public void BeFalseIfNonEnglish()
	{
		const Language language = Language.English;

		foreach (var c in GetFrenchSpecial().Concat(GetSpanishSpecial()))
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeFalse();
		}
	}

	[Fact]
	public void BeTrueForFrench()
	{
		const Language language = Language.French;

		foreach (var c in GetEnglishChars().Concat(GetFrenchSpecial()))
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeTrue();
		}
	}

	[Fact]
	public void BeFalseForNonFrench()
	{
		const Language language = Language.French;

		var chars = new[]
		{
			193,
			195,
			204,
			207,
			213,
			218,
			221
		};

		foreach (var c in ConvertToChar(chars).Concat(GetSpanishSpecial()))
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeFalse();
		}
	}

	[Fact]
	public void BeTrueForSpanish()
	{
		const Language language = Language.Spanish;

		foreach (var c in GetEnglishChars().Concat(GetSpanishSpecial()))
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeTrue();
		}
	}

	[Fact]
	public void BeFalseForNonSpanish()
	{
		const Language language = Language.Spanish;

		foreach (var c in GetFrenchSpecial())
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeFalse();
		}
	}

	[Fact]
	public void BeTrueForPortuguese()
	{
		const Language language = Language.Portuguese;

		foreach (var c in GetEnglishChars().Concat(GetPortugueseSpecial()))
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeTrue();
		}
	}

	[Fact]
	public void BeFalseForNonPortuguese()
	{
		const Language language = Language.Portuguese;

		var chars = new[]
		{
			196,
			200,
			203,
			206,
			214
		};

		foreach (var c in ConvertToChar(chars).Concat(GetSpanishSpecial()))
		{
			var result = c.IsLanguage(language);

			result
				.Should()
				.BeFalse();
		}
	}

	private static IEnumerable<char> GetEnglishChars()
	{
		for (var c = 'A'; c <= 'Z'; c++)
		{
			yield return c;
			yield return char.ToLower(c);
		}
	}

	private static IEnumerable<char> GetFrenchSpecial()
	{
		var chars = new[]
		{
			'À',
			'Â',
			'Æ',
			'Ç',
			'È',
			'É',
			'Ê',
			'Ë',
			'Î',
			'Ô',
			'Ù',
			'Û',
			'Ü',
			'Œ',
			'Ÿ'
		};

		for (var i = 0; i < chars.Length; i++)
		{
			yield return chars[i];
			yield return char.ToLower(chars[i]);
		}
	}

	private static IEnumerable<char> GetSpanishSpecial()
	{
		const char c = 'Ñ';

		yield return c;
		yield return char.ToLower(c);
	}

	private static IEnumerable<char> GetPortugueseSpecial()
	{
		var chars = new[]
		{
			'À',
			'Á',
			'É',
			'Í',
			'Ó',
			'Ú',
			'Â',
			'Ê',
			'Ô',
			'Ã',
			'Õ',
			'Ç'
		};

		for (var i = 0; i < chars.Length; i++)
		{
			yield return chars[i];
			yield return char.ToLower(chars[i]);
		}
	}

	private static IEnumerable<char> ConvertToChar(IReadOnlyList<int> ints)
	{
		for (var i = 0; i < ints.Count; i++)
		{
			var c = (char)ints[i];

			yield return c;
			yield return char.ToLower(c);
		}
	}
}