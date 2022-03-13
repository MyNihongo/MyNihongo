using MediatR;
using MyNihongo.WebApi.Auth;

namespace MyNihongo.WebApi.Tests.Controllers;

public abstract class ControllerTestsBase
{
	protected Mock<IMediator> MockMediator { get; } = new();

	protected Mock<IUserSession> MockUserSession { get; } = new();

	protected virtual void VerifyNoOtherCalls()
	{
		MockMediator.VerifyNoOtherCalls();
		MockUserSession.VerifyNoOtherCalls();
	}

	protected sealed class TestServerCallContext : ServerCallContext
	{
		private readonly Dictionary<object, object> _userState;

		private Metadata? ResponseHeaders { get; set; }

		private TestServerCallContext(Metadata requestHeaders, CancellationToken cancellationToken)
		{
			RequestHeadersCore = requestHeaders;
			CancellationTokenCore = cancellationToken;
			ResponseTrailersCore = new Metadata();
			AuthContextCore = new AuthContext(string.Empty, new Dictionary<string, List<AuthProperty>>());
			_userState = new Dictionary<object, object>();
		}

		protected override string MethodCore => "MethodName";

		protected override string HostCore => "HostName";

		protected override string PeerCore => "PeerName";

		protected override DateTime DeadlineCore { get; } = DateTime.Now.AddDays(100);

		protected override Metadata RequestHeadersCore { get; }

		protected override CancellationToken CancellationTokenCore { get; }

		protected override Metadata ResponseTrailersCore { get; }

		protected override Status StatusCore { get; set; }

		protected override WriteOptions WriteOptionsCore { get; set; } = new(WriteFlags.NoCompress);

		protected override AuthContext AuthContextCore { get; }

		protected override IDictionary<object, object> UserStateCore => _userState;

		protected override ContextPropagationToken CreatePropagationTokenCore(ContextPropagationOptions? options)
		{
			throw new NotImplementedException();
		}

		protected override Task WriteResponseHeadersAsyncCore(Metadata responseHeaders)
		{
			if (ResponseHeaders != null)
			{
				throw new InvalidOperationException("Response headers have already been written.");
			}

			ResponseHeaders = responseHeaders;
			return Task.CompletedTask;
		}

		public static TestServerCallContext Create(Metadata? requestHeaders = null, CancellationToken ct = default) =>
			new(requestHeaders ?? new Metadata(), ct);
	}
}