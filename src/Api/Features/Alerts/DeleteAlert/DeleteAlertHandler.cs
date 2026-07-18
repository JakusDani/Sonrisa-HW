using Api.Common.Data;

namespace Api.Features.Alerts.DeleteAlert;

public static class DeleteAlertHandler
{
    public static DeleteAlertResponse Handle(DeleteAlertCommand command, FakeDatabase database)
    {
        var deleted = database.RemoveAlert(command.Id);
        var response = new DeleteAlertResponse(deleted);
        return response;
    }
}
