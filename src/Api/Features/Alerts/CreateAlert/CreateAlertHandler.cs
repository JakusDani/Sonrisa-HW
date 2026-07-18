using Api.Common.Data;
using Api.Common.Domain;

namespace Api.Features.Alerts.CreateAlert;

public static class CreateAlertHandler
{
    public static CreateAlertResponse Handle(CreateAlertCommand command, FakeDatabase database)
    {
        if (!CreateAlertValidator.IsValid(command))
        {
            throw new ArgumentException("Alert name is required and at least one channel must be enabled.");
        }

        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            EventType = command.EventType,
            EnabledChannels = command.EnabledChannels,
            IsEnabled = true
        };

        var saved = database.AddAlert(alert);
        var response = new CreateAlertResponse(saved.Id, saved.Name, saved.EventType, saved.EnabledChannels, saved.IsEnabled);
        return response;
    }
}
