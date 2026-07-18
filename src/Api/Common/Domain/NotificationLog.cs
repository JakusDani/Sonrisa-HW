namespace Api.Common.Domain;

public record NotificationLog(
    Guid Id,
    Guid AlertId,
    string AlertName,
    ChannelType Channel,
    string Message,
    NotificationStatus Status,
    DateTime TimestampUtc);
