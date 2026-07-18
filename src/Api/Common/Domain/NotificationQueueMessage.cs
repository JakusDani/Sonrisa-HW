namespace Api.Common.Domain;

public record NotificationQueueMessage(
    Guid WorldEventId,
    Guid AlertId,
    string AlertName,
    ChannelType Channel,
    string Message);
