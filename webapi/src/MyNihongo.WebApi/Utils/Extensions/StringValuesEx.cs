using Microsoft.Extensions.Primitives;

namespace MyNihongo.WebApi.Utils.Extensions;

public static class StringValuesEx
{
	public static bool TryGetSingle(this StringValues @this, out string value)
	{
		if (@this.Count == 1 && !string.IsNullOrEmpty(@this[0]))
		{
			value = @this[0];
			return true;
		}

		value = string.Empty;
		return false;
	}
}