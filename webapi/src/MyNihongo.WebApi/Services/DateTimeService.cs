using MyNihongo.WebApi.Infrastructure;

namespace MyNihongo.WebApi.Services;

public sealed class DateTimeService : IDateTimeService
{
	private readonly IClock _clock;

	public DateTimeService(IClock clock)
	{
		_clock = clock;
	}

	public DateTime GetUtcNow() =>
		DateTime.UtcNow;

	public Instant GetInstant() =>
		_clock.GetCurrentInstant();
}