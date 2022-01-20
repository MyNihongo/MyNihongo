using Grpc.Core;

namespace MyNihongo.WebApi.Services;

public class GreeterService : Greeter.GreeterBase
{
	private readonly ILogger<GreeterService> _logger;
	private readonly IMediator _mediator;

	public GreeterService(
		ILogger<GreeterService> logger,
		IMediator mediator)
	{
		_logger = logger;
		_mediator = mediator;
	}

	public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) =>
		Task.FromResult(new HelloReply
		{
			Message = "Hello " + request.Name
		});

	public override async Task SayManyHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
	{
		await foreach (var item in _mediator.CreateStream(request, context.CancellationToken))
		{
			await responseStream.WriteAsync(item)
				.ConfigureAwait(false);
		}
	}
}