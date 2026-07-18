namespace Api.Common.Domain;

public static class AlertValidation
{
    public static bool IsValid(string name, IReadOnlyCollection<ChannelType> enabledChannels)
    {
        var valid = !string.IsNullOrWhiteSpace(name) && enabledChannels.Count > 0;
        return valid;
    }
}
