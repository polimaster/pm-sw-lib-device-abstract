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
public interface IHistoryReader<THistory> {
    
    /// <summary>
    /// Logger
    /// </summary>
    ILogger? Logger { get; set; }

    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    Action<HistoryChunk<THistory>>? HasNext  { get; set; }

    /// <summary>
    /// Device <see cref="IHistoryReader{THistory}"/> belongs to
    /// </summary>
    IDevice Device { get; set; }

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Array of history records</returns>
    Task Read(CancellationToken cancellationToken = new());

    /// <summary>
    /// Cancels history reading
    /// </summary>
    /// <returns></returns>
    Task Stop();
}