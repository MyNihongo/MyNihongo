namespace MyNihongo.WebApi.Infrastructure;

internal static class CharEx
{
	public static bool IsLanguage(this char @this, Language language)
	{
		if (char.IsDigit(@this) || @this is '.' or ' ')
			return true;

		return language switch
		{
			Language.English => @this is
				>= 'A' and <= 'Z' or
				>= 'a' and <= 'z',
			Language.French => @this is
				>= 'A' and <= 'Z' or
				>= 'a' and <= 'z' or
				'À' or 'Â' or >= 'Æ' and <= 'Ë' or 'Î' or 'Ô' or 'Ù' or 'Û' or 'Ü' or 'Ÿ' or 'Œ' or
				'à' or 'â' or >= 'æ' and <= 'ë' or 'î' or 'ô' or 'ù' or 'û' or 'ü' or 'ÿ' or 'œ',
			Language.Spanish => @this is
				>= 'A' and <= 'Z' or
				>= 'a' and <= 'z' or
				'Ñ' or 'ñ',
			Language.Portuguese => @this is
				>= 'A' and <= 'Z' or
				>= 'a' and <= 'z' or
				>= 'À' and <= 'Ã' or 'Ç' or 'É' or 'Ê' or 'Í' or >= 'Ó' and <= 'Õ' or 'Ú' or
				>= 'à' and <= 'ã' or 'ç' or 'é' or 'ê' or 'í' or >= 'ó' and <= 'õ' or 'ú',
			_ => throw new ArgumentOutOfRangeException(nameof(language), $"Unknown {nameof(Language)}: {language}")
		};
	}
}