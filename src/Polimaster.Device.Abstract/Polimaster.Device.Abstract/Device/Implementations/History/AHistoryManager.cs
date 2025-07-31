using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <inheritdoc />
public abstract class AHistoryManager<THistory, TStream> : IHistoryManager<THistory> where THistory : AHistoryRecord {
    /// <summary>
    ///
    /// </summary>
    protected ITransport<TStream> Transport { get; }

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
    /// <param name="transport"><see cref="ITransport{TStream}"/></param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
    protected AHistoryManager(ITransport<TStream> transport, ILoggerFactory? loggerFactory) {
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());

        Transport.Closing += Stop;
    }
}