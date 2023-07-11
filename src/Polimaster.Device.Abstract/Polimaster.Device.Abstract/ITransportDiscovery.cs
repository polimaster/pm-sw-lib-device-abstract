using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device discovery search for devices on particular interface (like IrDA or Bluetooth).
/// </summary>
/// <typeparam name="TData"><see cref="ITransport{TData,TConnectionParams}"/></typeparam>
/// <typeparam name="TConnectionParams"><see cref="ITransport{TData,TConnectionParams}"/></typeparam>
public interface ITransportDiscovery<TData, TConnectionParams> {

    /// <summary>
    /// Search for available devices
    /// </summary>
    /// <returns>List of devices transport</returns>
    Task Search(CancellationToken token);

    /// <summary>
    /// Occurs when device found on interface
    /// </summary>
    Action<IEnumerable<ITransport<TData, TConnectionParams>>>? Found { get; set; }

    /// <summary>
    /// Occurs when device detached from interface
    /// </summary>
    Action<IEnumerable<ITransport<TData, TConnectionParams>>>? Lost { get; set; }
}