using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history reader
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHistoryManager<THistory> where THistory : AHistoryRecord {

    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    public event Action<HistoryChunk<THistory>>? HasNext;

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    Task Read(CancellationToken token);

    /// <summary>
    /// Cancels history reading
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Wipe(CancellationToken token);
}