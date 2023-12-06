using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc />
public class HttpStream : NetworkStringStream {
    /// <inheritdoc />
    public HttpStream(NetworkStream stream, ILoggerFactory? loggerFactory = null) : base(stream, loggerFactory) {
    }
}