using Api.Common.Data;
using Api.Common.Domain;

namespace Api.Features.Alerts.UpdateAlert;

public static class UpdateAlertHandler
{
    public static UpdateAlertResponse? Handle(UpdateAlertCommand command, FakeDatabase database)
    {
        var updated = database.UpdateAlert(command.Id, existing => new Alert
        {
            Id = existing.Id,
            Name = command.Name,
            EventType = command.EventType,
            EnabledChannels = command.EnabledChannels,
            IsEnabled = command.IsEnabled
        });

        var response = updated is null
            ? null
            : new UpdateAlertResponse(updated.Id, updated.Name, updated.EventType, updated.EnabledChannels, updated.IsEnabled);
        return response;
    }
}
