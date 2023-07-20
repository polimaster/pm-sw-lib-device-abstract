using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history reader
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHistoryManager<THistory> {
    
    /// <summary>
    /// Logger
    /// </summary>
    ILogger? Logger { get; set; }

    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    Action<HistoryChunk<THistory>>? HasNext  { get; set; }

    /// <summary>
    /// Device <see cref="IHistoryManager{THistory}"/> belongs to
    /// </summary>
    IDevice Device { get; set; }

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    Task Read(CancellationToken cancellationToken = new());

    /// <summary>
    /// Cancels history reading
    /// </summary>
    /// <returns></returns>
    Task Stop();
    
    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Wipe(CancellationToken cancellationToken = new());
}