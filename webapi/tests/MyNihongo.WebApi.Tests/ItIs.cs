using System.Diagnostics;
using Newtonsoft.Json;

namespace MyNihongo.WebApi.Tests;

internal static class ItIs
{
	public static T JsonEquivalent<T>(T value) =>
		It.Is<T>(x => IsJsonEquivalent(x, value));

	private static bool IsJsonEquivalent<T>(T @this, T that)
	{
		var thisJson = JsonConvert.SerializeObject(@this);
		var thatJson = JsonConvert.SerializeObject(that);
		return IsEqual(thisJson, thatJson);
	}

	//private static bool IsEquivalent<T>(T @this, T that)
	//{
	//	try
	//	{
	//		@this
	//			.Should()
	//			.BeEquivalentTo(that);

	//		return true;
	//	}
	//	catch (Exception ex)
	//	{
	//		Debug.WriteLine(ex);
	//		return false;
	//	}
	//}

	private static bool IsEqual<T>(T @this, T that)
	{
		try
		{
			@this
				.Should()
				.Be(that);

			return true;
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			return false;
		}
	}
}