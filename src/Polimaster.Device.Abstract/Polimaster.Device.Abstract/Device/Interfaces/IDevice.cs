using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Interfaces;


/// <summary>
/// Device with identifier
/// </summary>
public interface IDevice : IDisposable {
    
    /// <summary>
    /// Unique identifier of device
    /// </summary>
    string Id { get; }
    
    public ILogger<IDevice>? Logger { get; set; }
    
    /// <summary>
    /// Indicates device is disconnected and will be removed from memory
    /// </summary>
    Action? IsDisposing { get; set; }
}

/// <summary>
/// Device base
/// </summary>
/// <typeparam name="T">Data type for <see cref="ITransport{T}"/> layer and internal builders</typeparam>
public interface IDevice<T> : IDevice {

    /// <summary>
    /// Instance of see <see cref="ICommandBuilder{TData}"/>
    /// </summary>
    ICommandBuilder<T> CommandBuilder { get; set; }

    /// <summary>
    /// Instance of <see cref="ISettingBuilder"/>
    /// </summary>
    ISettingBuilder SettingBuilder { get; set; }

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport{TData, TConnectionParams}"/>
    ITransport<T> Transport { get; set; }

    /// <summary>
    /// Device information data
    /// </summary>
    DeviceInfo DeviceInfo { get; }

    /// <summary>
    /// Read device information
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new());
    
    /// <summary>
    ///  Reads device settings.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates thru this properties and call <see cref="IDeviceSetting{T}.Read"/> on target property.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ReadSettings(CancellationToken cancellationToken = new());

    /// <summary>
    /// Writes settings to device.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates thru this properties and call <see cref="IDeviceSetting{T}.CommitChanges"/> on target property.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteSettings(CancellationToken cancellationToken = new());

    /// <summary>
    /// Builds device settings with <see cref="SettingBuilder"/>
    /// </summary>
    void BuildSettings();
    
    /// <summary>
    /// Search for <see cref="IDeviceSetting{T}"/> properties in device object
    /// </summary>
    /// <returns>Array of <see cref="PropertyInfo"/></returns>
    IEnumerable<PropertyInfo> GetDeviceSettingsProperties();
}
