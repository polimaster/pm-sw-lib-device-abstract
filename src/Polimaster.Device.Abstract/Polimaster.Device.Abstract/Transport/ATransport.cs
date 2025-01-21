using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Abstract transport layer
/// </summary>
public abstract class ATransport<TStream> : ITransport<TStream> {
    /// <summary>
    /// Underlying client
    /// </summary>
    public IClient<TStream> Client { get; }

    /// <summary>
    /// Set limit of threads to 1, witch can access to read/write operations at a time. 
    /// See <see cref="SyncStreamAccess"/>
    /// </summary>
    protected virtual SemaphoreSlim Semaphore { get; } = new(1, 1);

    /// <summary>
    /// If enabled, only one call of <see cref="ExecOnStream"/> or <see cref="Open"/> will be executed at a time
    /// </summary>
    protected virtual bool SyncStreamAccess => true;

    /// <summary>
    /// Amount of milliseconds to sleep after command execution
    /// </summary>
    protected virtual ushort Sleep => 1;

    /// <summary>
    /// Keep connection open until it's disposed
    /// </summary>
    protected virtual bool KeepOpen => false;

    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <inheritdoc />
    public event Action? Closing;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="client">See <see cref="IClient{TStream}"/></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(IClient<TStream> client, ILoggerFactory? loggerFactory) {
        Logger = loggerFactory?.CreateLogger(GetType());
        Client = client;
    }

    /// <inheritdoc />
    public virtual async Task Open(CancellationToken cancellationToken) {
        if (Client.Connected) return;
        if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
        try {
            Logger?.LogDebug("Open transport connection (async)");
            await Client.Open(cancellationToken);
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Close() {
        if (KeepOpen) return;
        if (SyncStreamAccess) Semaphore.Wait();
        try {
            Closing?.Invoke();
            Client.Close();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task ExecOnStream(Func<TStream, Task> action, CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Executing {Name}", nameof(ExecOnStream));

        try {
            await Open(cancellationToken);
            if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
            await Exec();
        } catch {
            Client.Reset();
            await Open(cancellationToken);
            await Exec();
            Close();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }

        return;

        async Task Exec() {
            var stream = Client.GetStream();
            await action.Invoke(stream);
            Thread.Sleep(Sleep);
        }

    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing transport connection");
        if (SyncStreamAccess) Semaphore.Wait();
        try {
            Closing?.Invoke();
            Client.Close();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
            Client.Dispose();
        }
    }
}