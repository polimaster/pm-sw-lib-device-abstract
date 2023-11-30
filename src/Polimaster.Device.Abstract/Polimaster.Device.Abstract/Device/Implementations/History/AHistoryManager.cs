using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <inheritdoc />
public abstract class AHistoryManager<THistory> : IHistoryManager<THistory> {
    protected ILoggerFactory? LoggerFactory { get; }
    protected ILogger? Logger { get; }
    
    protected AHistoryManager(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <inheritdoc />
    public abstract event Action<HistoryChunk<THistory>>? HasNext;

    /// <inheritdoc />
    public abstract Task Read(ITransport transport, CancellationToken token = new());

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public abstract Task Wipe(ITransport transport, CancellationToken token = new());
}