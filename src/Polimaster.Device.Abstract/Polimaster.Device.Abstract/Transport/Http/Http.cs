using System;
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
    public override void Open() {
        if (Client.Connected) return;
        Logger?.LogDebug("Open transport connection");
        var connected = Client.OpenAsync(ConnectionParams).Wait(ConnectionParams.Timeout);
        if (!connected) throw new TimeoutException($"Connection to {ConnectionParams.Ip}:{ConnectionParams.Port} timed out");
    }
}