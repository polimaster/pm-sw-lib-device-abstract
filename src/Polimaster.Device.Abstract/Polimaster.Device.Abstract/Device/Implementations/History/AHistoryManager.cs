using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <inheritdoc />
public abstract class AHistoryManager<T, THistory> : IHistoryManager<THistory> {
    /// <summary>
    ///
    /// </summary>
    protected ITransport<T> Transport { get; }

    /// <summary>
    /// Logger factory
    /// </summary>
    protected ILoggerFactory? LoggerFactory { get; }
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="loggerFactory"></param>
    protected AHistoryManager(ITransport<T> transport, ILoggerFactory? loggerFactory) {
        Transport = transport;
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger(GetType());

        Transport.Closing += Stop;
    }

    /// <inheritdoc />
    public abstract event Action<HistoryChunk<THistory>>? HasNext;

    /// <inheritdoc />
    public abstract Task Read(CancellationToken token = new());

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public abstract Task Wipe(CancellationToken token = new());
}