using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="T">Device type</typeparam>
public interface IDeviceManager<T> {
    
    /// <summary>
    /// Occurs when irda device connected
    /// </summary>
    event Action<T>? Connected;
    
    /// <summary>
    /// Occurs when irda device disconnected
    /// </summary>
    event Action<T>? Disconnected;
    
    
    /// <summary>
    /// Current connected devices
    /// </summary>
    List<T> Devices { get; set; }
    
    
    /// <summary>
    /// Refresh list of device
    /// </summary>
    Task RefreshConnectedDevices();
}