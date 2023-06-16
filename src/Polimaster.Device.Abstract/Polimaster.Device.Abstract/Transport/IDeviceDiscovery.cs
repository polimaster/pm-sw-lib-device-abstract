using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Transport;

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