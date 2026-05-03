using SmartMovieCatalog.Application.Abstractions.Time;

namespace SmartMovieCatalog.Application.Tests.TestSupport;

public sealed class FakeClock : IClock
{
    public DateTimeOffset UtcNow { get; set; } = new(2026, 5, 3, 2, 0, 0, TimeSpan.Zero);
}
