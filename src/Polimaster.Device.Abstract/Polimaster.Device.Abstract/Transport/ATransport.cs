using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport; 

public abstract class ATransport<TConnectionParams> : ITransport<TConnectionParams> {
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams}";
    public IClient<TConnectionParams> Client { get; protected set; }
    public TConnectionParams ConnectionParams { get; protected set; }
    
    protected readonly ILogger<ITransport<TConnectionParams>>? Logger;


    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="ConnectionParams" /></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) {
        ConnectionParams = connectionParams;
        Client = client;
        Logger = loggerFactory?.CreateLogger<ITransport<TConnectionParams>>();
    }

    public virtual Task<Stream> Open() {
        if (Client.Connected) return Task.FromResult(Client.GetStream());
        Logger?.LogDebug("Opening transport connection to device {A}", ConnectionParams);
        Client.Connect(ConnectionParams);
        return Task.FromResult(Client.GetStream());
    }
    public virtual Task Close() {
        Logger?.LogDebug("Closing transport connection to device {A}", ConnectionParams);
        Client.Close();
        return Task.CompletedTask;
    }
    
    public async Task<IWriter> GetWriter() {
        var stream = await Open();
        if (stream == null) throw new TransportException("Stream is null");
        var writer = new Writer(stream) { AutoFlush = true };
        return writer;
    }

    public async Task<IReader> GetReader() {
        var stream = await Open();
        if (stream == null) throw new TransportException("Stream is null");
        var reader = new Reader(stream);
        return reader;
    }
    
    public virtual void Dispose() {
        Close();
    }
}