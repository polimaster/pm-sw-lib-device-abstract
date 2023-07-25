using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport; 

public abstract class ATransport<TConnectionParams> : ITransport<TConnectionParams> {
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams}";
    public IClient<TConnectionParams> Client { get; protected set; }
    public TConnectionParams ConnectionParams { get; protected set; }
    
    // protected readonly ILogger<ITransport<TConnectionParams>>? Logger;


    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="ConnectionParams" /></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) {
        ConnectionParams = connectionParams;
        Client = client;
        Client.LoggerFactory = loggerFactory;
        // Logger = loggerFactory?.CreateLogger<ITransport<TConnectionParams>>();
    }

    public virtual Task<IDeviceStream> Open() {
        Client.Open(ConnectionParams);
        return Client.GetStream();
    }

    public virtual Task Close() {
        Client.Close();
        return Task.CompletedTask;
    }

    public virtual void Dispose() {
        Close();
        Client.Dispose();
    }
}