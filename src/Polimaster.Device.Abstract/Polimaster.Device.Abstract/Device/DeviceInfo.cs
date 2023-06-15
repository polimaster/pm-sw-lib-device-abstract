namespace Polimaster.Device.Abstract.Device;

/// <inheritdoc cref="IDeviceInfo"/>
public struct DeviceInfo : IDeviceInfo {
    
    /// <inheritdoc cref="IDeviceInfo"/>
    public string? Manufacturer { get; set; }
    
    /// <inheritdoc cref="IDeviceInfo"/>
    public string? Model { get; set; }
    
    /// <inheritdoc cref="IDeviceInfo"/>
    public string? Serial { get; set; }
    
    /// <inheritdoc cref="IDeviceInfo"/>
    public string? Firmware { get; set; }
    
    /// <inheritdoc cref="IDeviceInfo"/>
    public string? Hardware { get; set; }
}