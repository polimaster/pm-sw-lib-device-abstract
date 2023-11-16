using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class ATransport<TClient, TConnectionParams> : ITransport
    where TClient : class, IClient<TConnectionParams>, new() {
    
    /// <inheritdoc />
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams}";
    
    private readonly TClient _client;
    private SemaphoreSlim Semaphore { get; } = new(1,1);
    
    /// <summary>
    /// If enabled, only one call of <see cref="Exec"/> will be executed at a time
    /// </summary>
    protected virtual bool SyncStreamAccess => true;
    
    /// <summary>
    /// Amount of milliseconds to sleep after command execution
    /// </summary>
    protected virtual ushort Sleep => 1;

    /// <summary>
    /// Parameters for connection
    /// </summary>
    protected TConnectionParams ConnectionParams { get; }
    
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionParams">Parameters for connection</param>
    /// <param name="loggerFactory"></param>
    protected ATransport(TConnectionParams connectionParams, ILoggerFactory? loggerFactory = null) {
        ConnectionParams = connectionParams;
        Logger = loggerFactory?.CreateLogger(GetType());
        _client = new TClient();
    }

    

    /// <inheritdoc />
    public async Task OpenAsync() {
        if (_client.Connected) return;
        Logger?.LogDebug("Open transport connection (async)");
        await _client.OpenAsync(ConnectionParams);
    }

    /// <inheritdoc />
    public virtual void Open() {
        if(_client.Connected) return;
        Logger?.LogDebug("Open transport connection");
        _client.Open(ConnectionParams);
    }

    /// <inheritdoc />
    public virtual void Close() => _client.Close();

    /// <inheritdoc />
    public async Task Exec(ICommand command, CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Executing command {Name}", command.GetType().Name);
        if(SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
        try {
            var stream = await _client.GetStream();
            await command.Send(stream, Sleep, cancellationToken);
        } finally {
            if(SyncStreamAccess) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing transport connection");
        Close();
        _client.Dispose();
    }
}