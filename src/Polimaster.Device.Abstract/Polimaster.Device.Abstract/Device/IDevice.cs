using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

public interface IDevice : IDisposable {
    
    /// <summary>
    /// Unique identifier of device
    /// </summary>
    string Id { get; }
}

/// <summary>
/// Device base
/// </summary>
public interface IDevice<TData> : IDevice {
    
    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport{TData, TConnectionParams}"/>
    ITransport<TData> Transport { get; }
    
    DeviceInfo DeviceInfo { get; protected set; }
    
    /// <summary>
    /// Read device information
    /// </summary>
    /// <returns></returns>
    Task<DeviceInfo> ReadDeviceInfo();
    
    /// <summary>
    /// Indicates device is disconnected and will be removed from memory
    /// </summary>
    Action? IsDisposing { get; set; }
    
    /// <summary>
    ///  Reads device settings.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates thru this properties and call <see cref="IDeviceSetting{T}.Read()"/> on target property.
    /// </summary>
    /// <returns></returns>
    Task ReadSettings();

    /// <summary>
    /// Writes settings to device.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates thru this properties and call <see cref="IDeviceSetting{T}.CommitChanges"/> on target property.
    /// </summary>
    /// <returns></returns>
    Task WriteSettings();
    
    /// <summary>
    /// Search for <see cref="IDeviceSetting{T}"/> properties in device object
    /// </summary>
    /// <returns>Array of <see cref="PropertyInfo"/></returns>
    IEnumerable<PropertyInfo> GetDeviceSettingsProperties();
}
