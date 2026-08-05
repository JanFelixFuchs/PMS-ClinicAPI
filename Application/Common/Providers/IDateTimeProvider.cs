namespace Application.Common.Providers;

public interface IDateTimeProvider
{
    // Properties
    DateTime UtcNow { get; }
}