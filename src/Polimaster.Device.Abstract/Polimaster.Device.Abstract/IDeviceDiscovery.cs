using System.Collections.Generic;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TData"><see cref="ITransport{TData,TConnectionParams}"/></typeparam>
/// <typeparam name="TConnectionParams"><see cref="ITransport{TData,TConnectionParams}"/></typeparam>
public interface IDeviceDiscovery<TData, TConnectionParams> {

    /// <summary>
    /// Discover available devices
    /// </summary>
    /// <returns>List of devices transport</returns>
    IEnumerable<ITransport<TData, TConnectionParams>> Discover();
}