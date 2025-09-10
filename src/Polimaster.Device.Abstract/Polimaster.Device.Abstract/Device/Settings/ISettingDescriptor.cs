using System;
using System.Collections.Generic;

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
    /// Setting group name. On example, "Sound", "Display" etc.
    /// </summary>
    string? GroupName { get; }

    /// <summary>
    /// Units of measurement. Null if not applicable.
    /// </summary>
    object? Unit { get; }

    /// <summary>
    /// List of possible values
    /// </summary>
    IEnumerable<object>? ValueList { get; }
}