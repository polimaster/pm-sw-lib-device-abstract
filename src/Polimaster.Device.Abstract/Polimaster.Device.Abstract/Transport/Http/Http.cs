using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <summary>
/// Http transport implementation
/// </summary>
public class Http : ATransport<TcpClientAdapter, HttpConnectionParams>, IHttpTransport {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionParams"></param>
    /// <param name="loggerFactory"></param>
    public Http(HttpConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(connectionParams, loggerFactory) {
    }

    /// <inheritdoc />
    public override Task OpenAsync() {
        if (Client is { Connected: true }) return Client.GetStream();
        var connected = Client != null && Client.OpenAsync(ConnectionParams).Wait(ConnectionParams.Timeout);
        if (!connected)
            throw new TimeoutException($"Connection to {ConnectionParams.Ip}:{ConnectionParams.Port} timed out");
        return Client?.GetStream() ?? throw new InvalidOperationException();
    }
}