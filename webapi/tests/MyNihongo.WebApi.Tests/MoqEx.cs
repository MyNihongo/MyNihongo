using MediatR;
using MyNihongo.WebApi.Auth;

namespace MyNihongo.WebApi.Tests;

internal static class MoqEx
{
	public static void SetupInstant(this Mock<IDateTimeService> @this, int year, int monthOfYear, int dayOfMonth, int hourOfDay)
	{
		@this.Setup(x => x.GetInstant())
			.Returns(Instant.FromUtc(year, monthOfYear, dayOfMonth, hourOfDay, 0));
	}

	public static void SetupUtcNow(this Mock<IDateTimeService> @this)
	{
		@this.Setup(x => x.GetUtcNow())
			.Returns(DateTime.UtcNow);
	}

	public static void SetupUserId(this Mock<IUserSession> @this, long? userId)
	{
		@this.SetupGet(x => x.UserId)
			.Callback(() => @this.VerifyGet(x => x.UserId, Times.Once))
			.Returns(userId);
	}

	public static void SetupRequiredUserId(this Mock<IUserSession> @this, long userId)
	{
		@this.SetupGet(x => x.RequiredUserId)
			.Callback(() => @this.VerifyGet(x => x.RequiredUserId, Times.Once))
			.Returns(userId);
	}

	public static void SetupSend<TResponse>(this Mock<IMediator> @this, TResponse response)
	{
		@this.Setup(x => x.Send(It.IsAny<IRequest<TResponse>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(response);
	}

	public static void SetupCreateStream<TResponse>(this Mock<IMediator> @this, IReadOnlyList<TResponse> response)
	{
		@this.Setup(x => x.CreateStream(It.IsAny<IStreamRequest<TResponse>>(), It.IsAny<CancellationToken>()))
			.Returns(response.ToAsyncEnumerable());
	}

	public static void VerifyWrite<T>(this Mock<IServerStreamWriter<T>> @this, IEnumerable<T> expected)
	{
		foreach (var item in expected)
		{
			@this.Verify(x => x.WriteAsync(item), Times.Once);
		}
	}
}