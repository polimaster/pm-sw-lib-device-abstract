using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Abstract transport layer
/// </summary>
public abstract class ATransport : ITransport {
    /// <summary>
    /// Underlying client
    /// </summary>
    public IClient Client { get; }

    /// <summary>
    /// Set limit of threads to 1, witch can access to read/write operations at a time. 
    /// See <see cref="SyncStreamAccess"/>
    /// </summary>
    protected virtual SemaphoreSlim Semaphore { get; } = new(1, 1);

    /// <summary>
    /// If enabled, only one call of <see cref="ReadAsync"/> or <see cref="WriteAsync"/> will be executed at a time
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
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="loggerFactory"></param>
    protected ATransport(IClient client, ILoggerFactory? loggerFactory) {
        Logger = loggerFactory?.CreateLogger(GetType());
        Client = client;
    }

    /// <inheritdoc />
    public virtual async Task OpenAsync(CancellationToken cancellationToken) {
        if (Client.Connected) return;
        if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
        try {
            Logger?.LogDebug("Open transport connection (async)");
            await Client.OpenAsync(cancellationToken);
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }
    }

    /// <inheritdoc />
    public virtual void Open() {
        if (Client.Connected) return;
        if (SyncStreamAccess) Semaphore.Wait();
        try {
            Logger?.LogDebug("Open transport connection");
            Client.Open();
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
    public virtual async Task WriteAsync<T>(byte[] data, CancellationToken cancellationToken, T? channel = default) {
        Logger?.LogDebug("Executing {Name}", nameof(WriteAsync));
        try {
            await OpenAsync(cancellationToken);
            if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
            await Execute();
        } catch {
            Client.Reset();
            await OpenAsync(cancellationToken);
            await Execute();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }

        return;

        async Task Execute() {
            var stream = Client.GetStream();
            await stream.WriteAsync(data, cancellationToken, channel);
            Thread.Sleep(Sleep);
        }
    }

    /// <inheritdoc />
    public async Task WriteAsync(byte[] data, CancellationToken cancellationToken) {
        await WriteAsync<object>(data, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        return await ReadAsync<object>(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<byte[]> ReadAsync<T>(CancellationToken cancellationToken, T? channel = default) {
        Logger?.LogDebug("Executing {Name}", nameof(ReadAsync));

        try {
            await OpenAsync(cancellationToken);
            if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
            return await Execute();
        } catch {
            Client.Reset();
            await OpenAsync(cancellationToken);
            return await Execute();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }

        async Task<byte[]> Execute() {
            var stream = Client.GetStream();
            var res = await stream.ReadAsync(cancellationToken, channel);
            Thread.Sleep(Sleep);
            return res;
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