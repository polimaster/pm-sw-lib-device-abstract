using Polimaster.Device.Abstract;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.Usb;

/// <summary>
/// USB device connection properties
/// </summary>
public struct UsbDevice : IStringify {
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

    /// <inheritdoc cref="IFormatableIFormatablering" />
    public override string ToString() => $"{Name}:{PlugAndPlayId}";
}