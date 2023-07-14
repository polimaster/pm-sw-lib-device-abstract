using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Http;

public class Http : ATransport<HttpConnectionParams>, IHttpTransport {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="client">Transport client</param>
    /// <param name="connectionParams"></param>
    /// <param name="loggerFactory"></param>
    public Http(IClient<HttpConnectionParams> client, HttpConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }

    public override Task<Stream> Open() {
        if (Client.Connected) Client.GetStream();
        var connected = Client.ConnectAsync(ConnectionParams).Wait(ConnectionParams.Timeout);
        if (!connected)
            throw new TimeoutException($"Connection to {ConnectionParams.Ip}:{ConnectionParams.Port} timed out");
        return Task.FromResult(Client.GetStream());
    }
}