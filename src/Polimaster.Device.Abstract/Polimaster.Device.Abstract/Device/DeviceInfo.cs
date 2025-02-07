using System;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device information
/// </summary>
public struct DeviceInfo {

    /// <summary>
    /// Model of device. Eq "PM1703"
    /// </summary>
    public string? Model;

    /// <summary>
    /// Device modification. Eq "MO II BT"
    /// </summary>
    public string? Modification;

    /// <summary>
    /// Full model specification. Eq "PM1703 MOIIBT"
    /// </summary>
    public string ModelFull => $"{Model}{Modification}";

    /// <summary>
    /// Serial number
    /// </summary>
    public string? Serial;

    /// <summary>
    /// Global unique identifier for device
    /// </summary>
    public string? Id;

    /// <summary>
    /// Hardware version
    /// </summary>
    public Version? HardwareVersion;
    
    /// <summary>
    /// Firmware version
    /// </summary>
    public Version? FirmwareVersion;


    /// <summary>
    /// Date of manufacturing
    /// </summary>
    public DateTime? ManufacturingDate;

    /// <summary>
    /// Date of calibration
    /// </summary>
    public DateTime? CalibrationDate;
}