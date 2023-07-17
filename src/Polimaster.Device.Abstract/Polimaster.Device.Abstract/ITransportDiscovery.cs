using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device discovery search for devices on particular interface (like IrDA or Bluetooth).
/// </summary>
/// <typeparam name="TConnectionParams"><see cref="ITransport{TConnectionParams}"/></typeparam>
public interface ITransportDiscovery<TConnectionParams> : IDisposable {

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
    Action<IEnumerable<ITransport<TConnectionParams>>>? Found { get; set; }

    /// <summary>
    /// Occurs when device detached from interface
    /// </summary>
    Action<IEnumerable<ITransport<TConnectionParams>>>? Lost { get; set; }
}