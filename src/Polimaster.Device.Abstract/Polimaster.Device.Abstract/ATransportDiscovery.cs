using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ATransportDiscovery<TConnectionParams> : ITransportDiscovery<TConnectionParams> {
    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger? Logger;

    /// <summary>
    /// Tread sleep (milliseconds) between search iterations
    /// </summary>
    protected virtual int Sleep => 1000;

    /// <summary>
    /// Cancellation token source used in <see cref="Start"/>
    /// </summary>
    private CancellationTokenSource? _watchTokenSource;

    /// <summary>
    ///
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ATransportDiscovery(ILoggerFactory? loggerFactory) {
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <inheritdoc />
    public virtual void Start(CancellationToken token) {
        _watchTokenSource?.Cancel();
        _watchTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        Logger?.LogDebug("Starting device discovery");

        Task.Run(() => {
            while (true) {
                if (_watchTokenSource.Token.IsCancellationRequested) return Task.CompletedTask;
                try { Search(); } catch (Exception? e) { Logger?.LogError(e, "Cant search devices"); }
                Task.Delay(Sleep, _watchTokenSource.Token);
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
    public abstract event Action<IEnumerable<TConnectionParams>>? Found;

    /// <inheritdoc />
    public abstract event Action<IEnumerable<TConnectionParams>>? Lost;

    /// <inheritdoc />
    public virtual void Dispose() {
        Stop();
    }
}