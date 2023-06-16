using System;
using System.Collections.Generic;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="T">Device type</typeparam>
public interface IDeviceManager<T> {

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