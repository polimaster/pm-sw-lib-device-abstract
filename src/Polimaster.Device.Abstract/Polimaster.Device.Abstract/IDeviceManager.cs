using System;
using System.Collections.Generic;
using System.Threading;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="T">Device type</typeparam>
/// <typeparam name="TTransport"><see cref="ITransport{T}"/></typeparam>
public interface IDeviceManager<T, TTransport> : IDisposable where T : IDevice<TTransport> {

    /// <summary>
    /// Occurs when device attached to computer
    /// </summary>
    Action<T>? Attached { get; set; }
    
    /// <summary>
    /// Occurs when device detached from computer
    /// </summary>
    Action<T>? Removed { get; set; }

    /// <summary>
    /// Current connected devices
    /// </summary>
    List<T> Devices { get; set; }

    /// <summary>
    /// Starts finding devices in background
    /// </summary>
    /// <param name="token">Cancellation token</param>
    /// <param name="timeout">Cycle timeout (milliseconds)/></param>
    void StartDeviceDiscovery(CancellationToken token, int timeout = 20);

    /// <summary>
    /// Stops finding devices in background
    /// </summary>
    void StopDeviceDiscovery();
}