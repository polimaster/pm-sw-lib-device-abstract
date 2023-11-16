using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Interfaces;


/// <summary>
/// Device with identifier
/// </summary>
public interface IDevice : IDisposable, IEquatable<IDevice> {
    
    /// <summary>
    /// Unique identifier of device
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// Indicates device is disconnected and will be removed from memory
    /// </summary>
    Action? IsDisposing { get; set; }

    /// <summary>
    /// Device information data
    /// </summary>
    DeviceInfo? DeviceInfo { get; }

    /// <summary>
    /// Read device information
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken = new());
    
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
    /// Read/write device data with managed transport connection. Connection will be opened and closed after execution.
    /// This prevents situation when opened connection times out while idle and following calls throws exceptions.
    /// </summary>
    /// <example>
    /// await device.Execute(async () =&gt; {
    ///     await device.ReadDeviceInfo(stoppingToken);
    /// });
    /// </example>
    /// <param name="action">Function to call</param>
    /// <returns></returns>
    Task Execute(Func<Task> action);
    
    /// <summary>
    /// Search for <see cref="IDeviceSetting{T}"/> properties in device object
    /// </summary>
    /// <returns>Array of <see cref="PropertyInfo"/></returns>
    IEnumerable<PropertyInfo> GetDeviceSettingsProperties();
    
    /// <summary>
    /// Semaphore for exclusive access to device while sending commands.
    /// See <see cref="StringCommand{T}.Write"/> as example.
    /// </summary>
    // SemaphoreSlim Semaphore { get; }
}