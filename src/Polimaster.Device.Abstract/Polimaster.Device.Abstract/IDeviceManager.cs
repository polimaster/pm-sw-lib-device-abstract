using System;
using System.Collections.Generic;
using Polimaster.Device.Abstract.Device;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="T">Device type</typeparam>
public interface IDeviceManager<T> : IDisposable where T : IDevice {

    /// <summary>
    /// Occurs when device attached to computer
    /// </summary>
    public event Action<T>? Attached;

    /// <summary>
    /// Occurs when device detached from computer
    /// </summary>
    public event Action<T>? Removed;

    /// <summary>
    /// Current connected devices
    /// </summary>
    List<T> Devices { get; }
}