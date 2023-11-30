using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ATransportDiscovery : ITransportDiscovery {
    /// <summary>
    /// 
    /// </summary>
    protected readonly ILoggerFactory? LoggerFactory;

    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger? Logger;

    /// <summary>
    /// Tread sleep (milliseconds) between search iterations
    /// </summary>
    protected virtual int Sleep => 1000;

    private CancellationTokenSource? _watchTokenSource;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ATransportDiscovery(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <inheritdoc />
    public virtual void Start(CancellationToken token) {
        _watchTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        Logger?.LogDebug("Starting device discovery");

        Task.Run(() => {
            while (true) {
                if (_watchTokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
                try { Search(); } catch (Exception? e) { Logger?.LogError(e, "Cant search devices"); }
                Thread.Sleep(Sleep);
            }
        }, _watchTokenSource.Token);
    }

    /// <summary>
    /// Search devices
    /// </summary>
    protected abstract void Search();

    /// <inheritdoc />
    public virtual void Stop() {
        Logger?.LogDebug("Stopping device discovery");
        _watchTokenSource?.Cancel();
    }

    /// <inheritdoc />
    public Action<IEnumerable<ITransport>>? Found { get; set; }

    /// <inheritdoc />
    public Action<IEnumerable<ITransport>>? Lost { get; set; }

    /// <inheritdoc />
    public virtual void Dispose() {
        Stop();
    }
}