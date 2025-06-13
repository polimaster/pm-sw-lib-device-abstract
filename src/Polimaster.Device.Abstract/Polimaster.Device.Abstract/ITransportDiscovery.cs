using System;
using System.Collections.Generic;
using System.Threading;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Transport discovery searches for devices on a particular interface (like IrDA or Bluetooth).
/// </summary>
/// <typeparam name="TConnectionParams">Connection parameters</typeparam>
public interface ITransportDiscovery<out TConnectionParams> : IDisposable {

    /// <summary>
    /// Start search for available devices
    /// </summary>
    /// <returns>List of device's transport</returns>
    void Start(CancellationToken token);

    /// <summary>
    /// Stop search for available devices
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// Occurs when device found on interface
    /// </summary>
    public event Action<IEnumerable<TConnectionParams>>? Found;

    /// <summary>
    /// Occurs when the device detached from the interface
    /// </summary>
    public event Action<IEnumerable<TConnectionParams>>? Lost;
}