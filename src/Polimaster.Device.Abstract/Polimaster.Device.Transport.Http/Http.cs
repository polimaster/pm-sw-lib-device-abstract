using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Http;

/// <summary>
/// Http transport implementation
/// </summary>
public class Http : ATransport<TcpClientAdapter> {
    /// <inheritdoc />
    public Http(IClient<TcpClientAdapter> client, ILoggerFactory? loggerFactory) : base(client, loggerFactory) {
    }
}