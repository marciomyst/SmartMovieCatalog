namespace SmartMovieCatalog.Domain.Common;

public abstract record DomainEvent(DateTimeOffset OccurredOn);
