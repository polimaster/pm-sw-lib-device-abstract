using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Irda;

public class IrdaTransport<TConnectionParams> : ATransport<TConnectionParams>, IIrdaTransport<TConnectionParams> {

    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="IIrdaTransport{TConnectionParams}" /></param>
    /// <param name="loggerFactory"></param>
    public IrdaTransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }
    
}