using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <summary>
/// Http transport implementation
/// </summary>
public class Http : ATransport<TcpClientAdapter>, IHttpTransport {
    /// <inheritdoc />
    public Http(IClient<TcpClientAdapter> client, ILoggerFactory? loggerFactory) : base(client, loggerFactory) {
    }
}