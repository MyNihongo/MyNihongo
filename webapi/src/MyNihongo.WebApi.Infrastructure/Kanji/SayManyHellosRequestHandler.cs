using System.Runtime.CompilerServices;

namespace MyNihongo.WebApi.Infrastructure;

internal sealed class SayManyHellosRequestHandler : IStreamRequestHandler<HelloRequest, HelloReply>
{
	public async IAsyncEnumerable<HelloReply> Handle(HelloRequest request, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		for (int i = 0; i < 10; i++)
		{
			var item = new HelloReply
			{
				Message = "fuck you"
			};

			yield return item;
		}
	}
}