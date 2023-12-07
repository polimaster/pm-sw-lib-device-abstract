using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Http;

/// <summary>
/// Http transport implementation
/// </summary>
public class HttpTransport : ATransport<string> {
    /// <inheritdoc />
    protected override bool KeepOpen => true;

    /// <inheritdoc />
    public HttpTransport(IClient<string> client, ILoggerFactory? loggerFactory) : base(client, loggerFactory) {
    }
}