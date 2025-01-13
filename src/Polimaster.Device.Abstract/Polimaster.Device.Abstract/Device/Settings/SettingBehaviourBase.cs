namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc />
public class SettingBehaviourBase : ISettingBehaviour {

    /// <inheritdoc />
    public SettingAccessLevel AccessLevel { get; set; }

    /// <inheritdoc />
    public string? Name { get; init; }

    /// <inheritdoc />
    public string? Description { get; init; }

    /// <inheritdoc />
    public string? GroupName { get; init; }
}