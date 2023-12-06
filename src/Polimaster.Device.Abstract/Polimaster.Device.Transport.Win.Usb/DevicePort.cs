using System.IO.Ports;

namespace Polimaster.Device.Transport.Win.Usb;

/// <inheritdoc cref="IDevicePort" />
public class DevicePort : SerialPort, IDevicePort {
    /// <inheritdoc />
    public DevicePort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits) : 
        base(portName, baudRate, parity, dataBits, stopBits) {
    }
}