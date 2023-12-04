using System;

namespace Polimaster.Device.Transport.Win.Usb;

/// <summary>
/// USB device connection properties
/// </summary>
public struct UsbDevice : IFormattable {
    /// <summary>
    /// Device identifier. Usually COM port name.
    /// </summary>
    public readonly string Name;
    
    /// <summary>
    /// PlugAndPlay id
    /// </summary>
    public readonly string PlugAndPlayId;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="plugAndPlayId"></param>
    public UsbDevice(string name, string plugAndPlayId) {
        Name = name;
        PlugAndPlayId = plugAndPlayId;
    }

    /// <inheritdoc />
    public override string ToString() => ToString(null, System.Globalization.CultureInfo.CurrentCulture);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider formatProvider) => $"{Name}:{PlugAndPlayId}";
}