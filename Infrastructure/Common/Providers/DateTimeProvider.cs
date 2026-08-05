using Application.Common.Providers;

namespace Infrastructure.Common.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}