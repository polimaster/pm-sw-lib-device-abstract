using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history reader
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHistoryManager<THistory> {

    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    public event Action<HistoryChunk<THistory>>? HasNext;

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="token"></param>
    Task Read(CancellationToken token = new());

    /// <summary>
    /// Cancels history reading
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task Wipe(CancellationToken token = new());
}