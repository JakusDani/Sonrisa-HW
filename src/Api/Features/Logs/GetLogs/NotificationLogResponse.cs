using Api.Common.Domain;

namespace Api.Features.Logs.GetLogs;

public record NotificationLogResponse(Guid Id, Guid AlertId, string AlertName, ChannelType Channel, string Message, NotificationStatus Status, DateTime TimestampUtc);
