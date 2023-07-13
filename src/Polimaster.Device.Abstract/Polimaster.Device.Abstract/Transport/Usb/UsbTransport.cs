using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Usb; 

public class UsbTransport<TConnectionParams> : ATransport<TConnectionParams>, IUsbTransport<TConnectionParams> {

    public UsbTransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }
}