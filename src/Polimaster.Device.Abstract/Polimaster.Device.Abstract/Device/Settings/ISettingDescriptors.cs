using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// <see cref="ISettingDescriptor"/> container
/// </summary>
public interface ISettingDescriptors {
    /// <summary>
    /// Get all <see cref="ISettingDescriptor"/>s available for device type
    /// </summary>
    IEnumerable<ISettingDescriptor> GetAll();
}