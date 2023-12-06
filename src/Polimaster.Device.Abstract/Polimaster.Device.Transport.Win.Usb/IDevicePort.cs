using System.IO.Ports;

namespace Polimaster.Device.Transport.Win.Usb;

/// <summary>
/// <see cref="SerialPort"/> interface adapter
/// </summary>
public interface IDevicePort {
    /// <summary>
    /// See <see cref="SerialPort.WriteLine"/>
    /// </summary>
    void WriteLine(string buffer);

    /// <summary>
    /// See <see cref="SerialPort.ReadTo"/>
    /// </summary>
    string ReadTo(string value);

    /// <summary>
    /// See <see cref="SerialPort.NewLine"/>
    /// </summary>
    string NewLine { get; set; }
}