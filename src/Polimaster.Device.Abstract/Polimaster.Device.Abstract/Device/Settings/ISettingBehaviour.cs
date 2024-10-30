namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Define behaviour of device setting
/// </summary>
public interface ISettingBehaviour {
    /// <summary>
    /// Access level for setting. See <see cref="SettingAccessLevel"/>.
    /// </summary>
    SettingAccessLevel AccessLevel { get; }

    /// <summary>
    /// Setting group name. On example, "Sound", "Behaviour", "Gamma" etc.
    /// </summary>
    string? GroupName { get; }
}