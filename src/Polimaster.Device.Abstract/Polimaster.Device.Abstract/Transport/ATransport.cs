using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class ATransport<TClient, T> : ITransport<T>
    where TClient : class, IClient<T>, new() {
    
    /// <inheritdoc />
    public string ConnectionId => $"{GetType().Name}:{Client}";

    /// <summary>
    /// Underlying client
    /// </summary>
    protected readonly TClient Client;
    private SemaphoreSlim Semaphore { get; } = new(1,1);
    
    /// <summary>
    /// If enabled, only one call of <see cref="Exec{TValue}"/> will be executed at a time
    /// </summary>
    protected virtual bool SyncStreamAccess => true;
    
    /// <summary>
    /// Amount of milliseconds to sleep after command execution
    /// </summary>
    protected virtual ushort Sleep => 1;
    
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(TClient client, ILoggerFactory? loggerFactory = null) {
        Logger = loggerFactory?.CreateLogger(GetType());
        Client = client;
    }
    

    /// <inheritdoc />
    public virtual async Task OpenAsync() {
        if (Client.Connected) return;
        Logger?.LogDebug("Open transport connection (async)");
        await Client.OpenAsync();
    }

    /// <inheritdoc />
    public virtual void Open() {
        if(Client.Connected) return;
        Logger?.LogDebug("Open transport connection");
        Client.Open();
    }

    /// <inheritdoc />
    public virtual void Close() => Client.Close();

    /// <inheritdoc />
    public virtual async Task Exec<TValue>(ICommand<TValue, T> command, TValue? value = default, CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Executing command {Name}", command.GetType().Name);
        if(SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
        try {
            var stream = await Client.GetStream();
            await command.Send(stream, value, cancellationToken);
            Thread.Sleep(Sleep);
        } finally {
            if(SyncStreamAccess) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing transport connection");
        Close();
        Client.Dispose();
    }
}