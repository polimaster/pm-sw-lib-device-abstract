using System;
using System.Collections.Generic;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

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
    event Action<T>? Attached;
    
    /// <summary>
    /// Occurs when device detached from computer
    /// </summary>
    event Action<T>? Removed;
    
    
    /// <summary>
    /// Current connected devices
    /// </summary>
    List<T> Devices { get; set; }
}