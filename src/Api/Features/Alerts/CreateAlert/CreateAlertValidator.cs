namespace Api.Features.Alerts.CreateAlert;

public static class CreateAlertValidator
{
    public static bool IsValid(CreateAlertCommand command)
    {
        var valid = !string.IsNullOrWhiteSpace(command.Name) && command.EnabledChannels.Count > 0;
        return valid;
    }
}
