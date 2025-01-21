using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport.Socket;

namespace Polimaster.Device.Abstract.Tests.Impl.Transport;

public class MyDeviceStream(ISocketStream stream, ILoggerFactory? loggerFactory = null)
    : SocketByteStream(stream, loggerFactory);