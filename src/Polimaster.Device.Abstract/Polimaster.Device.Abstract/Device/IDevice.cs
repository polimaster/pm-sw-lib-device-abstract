using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;


/// <summary>
/// Device interface
/// </summary>
public interface IDevice {
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
    /// Reads device settings.
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
    /// Set setting by its <see cref="ISettingDescriptor"/>
    /// </summary>
    /// <param name="descriptor">Setting descriptor</param>
    /// <param name="value">Setting value</param>
    /// <typeparam name="T">Type of setting value</typeparam>
    IDeviceSetting SetSetting<T>(ISettingDescriptor descriptor, T value) where T : notnull;

    /// <summary>
    /// Get setting by its <see cref="ISettingDescriptor"/>
    /// </summary>
    /// <param name="descriptor"></param>
    /// <returns></returns>
    IDeviceSetting GetSetting(ISettingDescriptor descriptor);

    /// <summary>
    /// Get settings
    /// </summary>
    /// <returns></returns>
    IEnumerable<IDeviceSetting> GetSettings();
}

/// <summary>
/// Device with identifier
/// </summary>
public interface IDevice<TTransport, TStream> : IDevice, IDisposable, IEquatable<IDevice<TTransport, TStream>> where TTransport : ITransport<TStream> {
    /// <summary>
    /// Check if the device has the same <typeparamref name="TTransport"/>
    /// </summary>
    /// <param name="transport"></param>
    /// <returns></returns>
    bool HasSame(TTransport transport);
}