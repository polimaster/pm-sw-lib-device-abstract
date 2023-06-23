﻿using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements; 

/// <summary>
/// Identifies a device with battery
/// </summary>
public interface IHasBattery {
    
    /// <summary>
    /// Status of device battery
    /// </summary>
    BatteryStatus BatteryStatus { get; }
    
    /// <summary>
    /// Read battery status from device
    /// </summary>
    /// <returns><see cref="BatteryStatus"/></returns>
    Task<BatteryStatus> RefreshBatteryStatus();
}

/// <summary>
/// Status of device battery
/// </summary>
public struct BatteryStatus {
    
    /// <summary>
    /// Value in Volts
    /// </summary>
    public double? Volts;
    
    /// <summary>
    /// Value in percents
    /// </summary>
    public double? Percents;
}