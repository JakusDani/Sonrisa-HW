namespace Api.Common.Domain;

public record WorldEvent(Guid Id, EventType EventType, string Message, DateTime OccurredAtUtc);
