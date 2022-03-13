namespace MyNihongo.WebApi.Services;

public interface IDateTimeService
{
	DateTime GetUtcNow();

	Instant GetInstant();
}