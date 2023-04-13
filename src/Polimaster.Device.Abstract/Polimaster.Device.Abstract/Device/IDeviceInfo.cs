namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device information
/// </summary>
public interface IDeviceInfo {
    /// <summary>
    /// Device manufacturer
    /// </summary>
    string? Manufacturer { get; set; }

    /// <summary>
    /// Model of device
    /// </summary>
    string? Model { get; set; }

    /// <summary>
    /// Serial number
    /// </summary>
    string? Serial { get; set; }

    /// <summary>
    /// Firmware number or ID
    /// </summary>
    string? Firmware { get; set; }

    /// <summary>
    /// Hardware number or ID
    /// </summary>
    string? Hardware { get; set; }
}