using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream.Socket;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport;

public class MyDeviceStream(ISocketStream stream, ILoggerFactory? loggerFactory = null)
    : SocketStringStream(stream, loggerFactory);