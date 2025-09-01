using System;

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
    /// Type of underlying setting value
    /// </summary>
    Type ValueType { get; }

    /// <summary>
    /// Setting name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Setting description
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Setting group name. On example, "Sound", "Gamma" etc.
    /// </summary>
    string? GroupName { get; }
}