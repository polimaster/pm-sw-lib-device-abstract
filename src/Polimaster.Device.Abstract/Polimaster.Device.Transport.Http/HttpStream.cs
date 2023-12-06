using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream.Socket;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc />
public class HttpStream : SocketStringStream {
    /// <inheritdoc />
    public HttpStream(ISocketStream stream, ILoggerFactory? loggerFactory = null) : base(stream, loggerFactory) {
    }
}