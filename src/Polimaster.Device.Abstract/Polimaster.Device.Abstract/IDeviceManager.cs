using System;
using System.Collections.Generic;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract;

/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="T">Device type</typeparam>
public interface IDeviceManager<T> : IDisposable where T : IDevice {

    /// <summary>
    /// Occurs when device attached to computer
    /// </summary>
    Action<T>? Attached { get; set; }
    
    /// <summary>
    /// Occurs when device detached from computer
    /// </summary>
    Action<T>? Removed { get; set; }

    /// <summary>
    /// Current connected devices
    /// </summary>
    List<T> Devices { get; set; }
}