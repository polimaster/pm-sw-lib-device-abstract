﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Abstract transport layer
/// </summary>
/// <typeparam name="T">Type of <see cref="IClient{T}"/> data</typeparam>
public abstract class ATransport<T> : ITransport {
    /// <inheritdoc />
    public string ConnectionId => $"{GetType().Name}:{Client}";

    /// <summary>
    /// Underlying client
    /// </summary>
    protected readonly IClient<T> Client;

    /// <summary>
    /// Set limit of threads to 1, witch can access to read/write operations at a time. 
    /// See <see cref="SyncStreamAccess"/>
    /// </summary>
    protected virtual SemaphoreSlim Semaphore { get; } = new(1, 1);

    /// <summary>
    /// If enabled, only one call of <see cref="Exec"/> will be executed at a time
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
    protected ATransport(IClient<T> client, ILoggerFactory? loggerFactory) {
        Logger = loggerFactory?.CreateLogger(GetType());
        Client = client;
    }


    /// <param name="cancellationToken"></param>
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
    public virtual async Task Exec(ICommand command, CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Executing command {Name}", command.GetType().Name);
        if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);

        try { await Execute(); } catch {
            Client.Reset();
            await Client.OpenAsync(cancellationToken);
            await Execute();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }

        return;

        async Task Execute() {
            var stream = Client.GetStream();
            await command.Exec(stream, cancellationToken);
            Thread.Sleep(Sleep);
        }
    }


    /// <inheritdoc />
    public virtual async Task Write<TData>(IDataWriter<TData> writer, TData data, CancellationToken cancellationToken) {
        Logger?.LogDebug("Executing {Name}", writer.GetType().Name);
        if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
        try { await Execute(); } catch {
            Client.Reset();
            await Client.OpenAsync(cancellationToken);
            await Execute();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }

        return;

        async Task Execute() {
            var stream = Client.GetStream();
            await writer.Write(stream, data, cancellationToken);
            Thread.Sleep(Sleep);
        }
    }

    /// <inheritdoc />
    public virtual async Task<TData> Read<TData>(IDataReader<TData> reader, CancellationToken cancellationToken) {
        Logger?.LogDebug("Executing {Name}", reader.GetType().Name);
        if (SyncStreamAccess) await Semaphore.WaitAsync(cancellationToken);
        try { return await Execute(); } catch {
            Client.Reset();
            await Client.OpenAsync(cancellationToken);
            return await Execute();
        } finally {
            if (SyncStreamAccess) Semaphore.Release();
        }

        async Task<TData> Execute() {
            var stream = Client.GetStream();
            var res = await reader.Read(stream, cancellationToken);
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

    /// <inheritdoc />
    public bool Equals(ITransport other) {
        return ConnectionId.Equals(other.ConnectionId);
    }
}