using Api.Common.Domain;

namespace Api.Features.Alerts.UpdateAlert;

public static class UpdateAlertValidator
{
    public static bool IsValid(UpdateAlertCommand command)
    {
        var valid = AlertValidation.IsValid(command.Name, command.EnabledChannels);
        return valid;
    }
}
