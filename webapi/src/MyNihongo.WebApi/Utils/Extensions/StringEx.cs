namespace MyNihongo.WebApi.Utils.Extensions;

public static class StringEx
{
	public static bool HasSequence(this string @this, in string that)
	{
		if (@this.Length < that.Length)
			return false;

		for (var i = 0; i < that.Length; i++)
			if (@this[i] != that[i])
				return false;

		return true;
	}
}