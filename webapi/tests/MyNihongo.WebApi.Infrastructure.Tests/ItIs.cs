using System.Diagnostics;

namespace MyNihongo.WebApi.Infrastructure.Tests;

internal static class ItIs
{
	public static T Equivalent<T>(T value) =>
		It.Is<T>(x => IsEquivalent(x, value));

	private static bool IsEquivalent<T>(T @this, T that)
	{
		try
		{
			@this
				.Should()
				.BeEquivalentTo(that);

			return true;
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			return false;
		}
	}
}