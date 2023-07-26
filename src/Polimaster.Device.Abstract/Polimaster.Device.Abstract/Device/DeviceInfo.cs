using System;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device information
/// </summary>
public struct DeviceInfo {

    /// <summary>
    /// Model of device
    /// </summary>
    public string? Model;

    /// <summary>
    /// Serial number
    /// </summary>
    public string? Serial;

    /// <summary>
    /// Firmware number or ID
    /// </summary>
    public string? Firmware;

    /// <summary>
    /// Hardware version
    /// </summary>
    public Version? HardwareVersion;
    
    /// <summary>
    /// Firmware version
    /// </summary>
    public Version? FirmwareVersion;
}