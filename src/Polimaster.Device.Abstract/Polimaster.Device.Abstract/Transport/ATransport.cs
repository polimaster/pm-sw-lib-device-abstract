using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport; 

public abstract class ATransport<T, TConnectionParams> : ITransport<T, TConnectionParams> {
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams}";
    public abstract Task Write(Stream stream, T command, CancellationToken cancellationToken);
    public abstract Task<T> Read(Stream stream, T command, CancellationToken cancellationToken);
    public abstract Task<Stream?> Open();
    public abstract Task Close();
    public IClient<TConnectionParams> Client { get; protected set; }
    public TConnectionParams ConnectionParams { get; protected set; }
    
    protected readonly ILogger<ITransport<T, TConnectionParams>>? Logger;


    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="ConnectionParams" /></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) {
        ConnectionParams = connectionParams;
        Client = client;
        Logger = loggerFactory?.CreateLogger<ITransport<T, TConnectionParams>>();
    }
    
    public abstract void Dispose();
}