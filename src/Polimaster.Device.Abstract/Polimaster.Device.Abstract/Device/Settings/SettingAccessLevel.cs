namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Device setting access level
/// </summary>
public enum SettingAccessLevel {
    /// <summary>
    /// Base level with no restrictions.
    /// </summary>
    BASE = 0,

    /// <summary>
    /// Level requires any type of authentication to change setting. With password, on example.
    /// </summary>
    EXTENDED = 1,

    /// <summary>
    /// Level requires technical knowledge to modify device setting.
    /// Use it for settings that can be changed in technological mode of application.
    /// </summary>
    ADVANCED = 2
}