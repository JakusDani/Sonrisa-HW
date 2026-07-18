using Api.Common.Domain;

namespace Api.Features.Alerts.CreateAlert;

public record CreateAlertCommand(string Name, EventType EventType, IReadOnlyCollection<ChannelType> EnabledChannels);
