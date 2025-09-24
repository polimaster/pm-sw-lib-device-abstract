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
    /// Set the limit of threads to 1, which can access to read/write operations at a time.
    /// See <see cref="SyncStreamAccess"/>
    /// </summary>
    protected virtual SemaphoreSlim Semaphore { get; } = new(1, 1);

    /// <summary>
    /// If enabled, only one call of <see cref="ExecOnStream"/> or <see cref="Open"/> will be executed at a time
    /// </summary>
    protected virtual bool SyncStreamAccess => true;

    /// <summary>
    /// Number of milliseconds to sleep after command execution
    /// </summary>
    protected virtual ushort Sleep => 1;

    /// <summary>
    /// Keep the connection open until it's disposed
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
        var locked = false;
        try {
            if (SyncStreamAccess) {
                await Semaphore.WaitAsync(cancellationToken);
                locked = true;
            }
            Logger?.LogDebug("Open transport connection (async)");
            await Client.Open(cancellationToken);
        } finally {
            if (SyncStreamAccess && locked) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Close() {
        if (KeepOpen) return;
        var locked = false;
        try {
            if (SyncStreamAccess) {
                Semaphore.Wait();
                locked = true;
            }
            Closing?.Invoke();
            Client.Close();
        } finally {
            if (SyncStreamAccess && locked) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual async Task ExecOnStream(Func<TStream, Task> action, CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Executing {Name}", nameof(ExecOnStream));

        var locked = false;
        try {
            await Open(cancellationToken);
            if (SyncStreamAccess) {
                await Semaphore.WaitAsync(cancellationToken);
                locked = true;
            }
            await Exec();
        } catch {
            Client.Reset();
            await Open(cancellationToken);
            await Exec();
            Close();
        } finally {
            if (SyncStreamAccess && locked) Semaphore.Release();
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
        var locked = false;

        try {
            if (SyncStreamAccess) {
                Semaphore.Wait();
                locked = true;
            }
            Closing?.Invoke();
            Client.Close();
        } finally {
            if (SyncStreamAccess && locked) Semaphore.Release();
            Client.Dispose();
        }
    }
}