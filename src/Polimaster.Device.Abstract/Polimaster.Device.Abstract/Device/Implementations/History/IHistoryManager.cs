using System;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history reader
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHistoryManager<THistory> {

    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    Action<HistoryChunk<THistory>>? HasNext { get; set; }

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="token"></param>
    Task Read(ITransport transport, CancellationToken token = new());

    /// <summary>
    /// Cancels history reading
    /// </summary>
    /// <returns></returns>
    void Stop();

    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task Wipe(ITransport transport, CancellationToken token = new());
}