using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;


/// <summary>
/// Device with identifier
/// </summary>
public interface IDevice<TTransport, TStream> : IDisposable, IEquatable<IDevice<TTransport, TStream>> where TTransport : ITransport<TStream> {
    
    /// <summary>
    /// Unique identifier of device
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Indicates device is disconnected and will be removed from memory
    /// </summary>
    public event Action? IsDisposing;

    /// <summary>
    /// Device information data
    /// </summary>
    DeviceInfo? DeviceInfo { get; }

    /// <summary>
    /// Read device information
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken);
    
    /// <summary>
    ///  Reads device settings.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates properties and call <see cref="IDeviceSetting{T}.Read"/> on target property.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task ReadAllSettings(CancellationToken cancellationToken);

    /// <summary>
    /// Writes settings to device.
    /// Successor class should have properties of type <see cref="IDeviceSetting{T}"/> interface.
    /// Method iterates <see cref="IDevice{TTransport,TStream}"/> properties and call <see cref="IDeviceSetting{T}.CommitChanges"/> on target property.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteAllSettings(CancellationToken cancellationToken);

    /// <summary>
    /// Returns device settings
    /// </summary>
    /// <returns></returns>
    IEnumerable<PropertyInfo> GetSettings();

    /// <summary>
    /// Check if device has the same <typeparamref name="TTransport"/>
    /// </summary>
    /// <param name="transport"></param>
    /// <returns></returns>
    bool HasSame(TTransport transport);
}