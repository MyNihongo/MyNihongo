namespace MyNihongo.Tests.GrpcClient;

public static class Assertions
{
	public static async Task ThrowUnauthorisedAsync<T>(this GenericAsyncFunctionAssertions<T> @this)
	{
		var result = await @this
			.ThrowExactlyAsync<RpcException>();

		result
			.And
			.StatusCode
			.Should()
			.Be(StatusCode.Unauthenticated);
	}
}