namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Description of device setting
/// </summary>
public interface ISettingDescriptor {
    /// <summary>
    /// Access level for setting. See <see cref="SettingAccessLevel"/>.
    /// </summary>
    SettingAccessLevel AccessLevel { get; set; }

    /// <summary>
    /// Setting name
    /// </summary>
    string? Name { get; init; }

    /// <summary>
    /// Setting description
    /// </summary>
    string? Description { get; init; }

    /// <summary>
    /// Setting group name. On example, "Sound", "Behaviour", "Gamma" etc.
    /// </summary>
    string? GroupName { get; init; }
}