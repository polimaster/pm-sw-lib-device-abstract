using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;


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
    Task ReadAllSettings(CancellationToken cancellationToken = new());

    /// <summary>
    /// Writes settings to device.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates thru this properties and call <see cref="IDeviceSetting{T}.CommitChanges"/> on target property.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteAllSettings(CancellationToken cancellationToken = new());

    /// <summary>
    /// Read/write device data with managed transport connection. Connection will be opened and closed after execution.
    /// This prevents situation when opened connection times out while idle and following calls throws exceptions.
    /// </summary>
    /// <example>
    /// await device.Execute(async (transport) =&gt; {
    ///     await transport.Read(reader);
    /// });
    /// </example>
    /// <param name="action">Function to call</param>
    /// <returns></returns>
    Task Execute(Func<ITransport, Task> action);
}