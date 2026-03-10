using System.ComponentModel;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Represents the On/Off state as an enumeration.
/// </summary>
public enum OnOff {
    /// <summary>
    /// Represents the "On" state in the <c>OnOff</c> enumeration,
    /// indicating that the corresponding device setting or functionality is active or enabled.
    /// </summary>
    [Description("On")] ON,

    /// <summary>
    /// Represents the "Off" state in the <c>OnOff</c> enumeration,
    /// indicating that the corresponding device setting or functionality is inactive or disabled.
    /// </summary>
    [Description("Off")] OFF
}