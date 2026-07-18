using Api.Common.Domain;

namespace Api.Features.Admin.SimulateEvent;

public record SimulateEventCommand(EventType EventType, string Message);
