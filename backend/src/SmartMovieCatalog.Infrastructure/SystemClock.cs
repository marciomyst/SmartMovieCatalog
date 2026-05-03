using SmartMovieCatalog.Application.Abstractions.Time;

namespace SmartMovieCatalog.Infrastructure;

public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
