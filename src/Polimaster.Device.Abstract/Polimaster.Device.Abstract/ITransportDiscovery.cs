using System;
using System.Collections.Generic;
using System.Threading;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Transport discovery searches for devices on particular interface (like IrDA or Bluetooth).
/// </summary>
/// <typeparam name="TConnectionParams">Connection parameters</typeparam>
public interface ITransportDiscovery<out TConnectionParams> : IDisposable {

    /// <summary>
    /// Start search for available devices
    /// </summary>
    /// <returns>List of devices transport</returns>
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
    /// Occurs when device detached from interface
    /// </summary>
    public event Action<IEnumerable<TConnectionParams>>? Lost;
}