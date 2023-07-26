using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history reader
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHistoryManager<THistory> {

    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    Action<HistoryChunk<THistory>>? HasNext  { get; set; }

    /// <summary>
    /// Device <see cref="IHistoryManager{THistory}"/> belongs to
    /// </summary>
    IDevice Device { get; }

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    void Read(CancellationToken cancellationToken = new());

    /// <summary>
    /// Cancels history reading
    /// </summary>
    /// <returns></returns>
    void Stop();
    
    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Wipe(CancellationToken cancellationToken = new());
}