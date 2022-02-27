namespace MyNihongo.WebApi.Infrastructure;

internal static class DoubleOptionalEx
{
	public static byte? GetByteRating(this DoubleOptional? @this)
	{
		double? doubleValue = @this;
		switch (doubleValue)
		{
			case null:
			case < 0d:
			case > 5d:
				return null;
			default:
				doubleValue = Math.Round(doubleValue.Value, 1);
				return (byte)(doubleValue * 10d);
		}
	}
}