using System.Collections.Generic;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device discovery search for devices on particular interface (like IrDA or Bluetooth).
/// </summary>
/// <typeparam name="TData"><see cref="ITransport{TData,TConnectionParams}"/></typeparam>
/// <typeparam name="TConnectionParams"><see cref="ITransport{TData,TConnectionParams}"/></typeparam>
public interface IDeviceDiscovery<TData, TConnectionParams> {

    /// <summary>
    /// Search for available devices
    /// </summary>
    /// <returns>List of devices transport</returns>
    IEnumerable<ITransport<TData, TConnectionParams>> Search();
}