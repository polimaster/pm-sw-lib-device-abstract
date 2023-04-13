using System;
using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device; 

/// <summary>
/// Device observer interface 
/// </summary>
/// <typeparam name="TDeviceObservable">Type of discovered device</typeparam>
public interface IDeviceObserver<TDeviceObservable> {
    
    /// <summary>
    /// On device connected event
    /// </summary>
    event Action<TDeviceObservable> Connected;
    
    /// <summary>
    /// On device disconnected event
    /// </summary>
    event Action<TDeviceObservable> Disconnected;
    
    /// <summary>
    /// List of currently connected devices
    /// </summary>
    List<TDeviceObservable> ConnectedDevices { get; }
}