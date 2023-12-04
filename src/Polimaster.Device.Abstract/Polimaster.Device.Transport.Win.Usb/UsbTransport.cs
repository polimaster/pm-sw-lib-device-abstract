using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.Usb;

/// <summary>
/// 
/// </summary>
public class UsbTransport : ATransport<SerialPortAdapter> {
    /// <inheritdoc />
    public UsbTransport(IClient<SerialPortAdapter> client, ILoggerFactory? loggerFactory) : base(client, loggerFactory) {
    }
}