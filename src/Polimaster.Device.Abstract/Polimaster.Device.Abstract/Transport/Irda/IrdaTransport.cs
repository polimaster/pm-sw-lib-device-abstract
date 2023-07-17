using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Irda;

public class IrdaTransport<TConnectionParams> : ATransport<TConnectionParams>, IIrdaTransport<TConnectionParams> {

    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="IIrdaTransport{TConnectionParams}" /></param>
    /// <param name="loggerFactory"></param>
    protected IrdaTransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }
    
    // public override async Task<Stream> Open() {
    //     if (Client.Connected) return Client.GetStream();
    //     Logger?.LogDebug("Opening transport connection to device {A}", ConnectionParams);
    //     await Client.ConnectAsync(ConnectionParams);
    //     return Client.GetStream();
    // }
    
}