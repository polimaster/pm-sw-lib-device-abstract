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
    /// Logger
    /// </summary>
    protected readonly ILogger? Logger;

    /// <inheritdoc />
    public abstract event Action<HistoryChunk<THistory>>? HasNext;

    /// <inheritdoc />
    public abstract Task Read(CancellationToken token);

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public abstract Task Wipe(CancellationToken token);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="transport"><see cref="ITransport{T}"/></param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
    protected AHistoryManager(ITransport<T> transport, ILoggerFactory? loggerFactory) {
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());

        Transport.Closing += Stop;
    }
}