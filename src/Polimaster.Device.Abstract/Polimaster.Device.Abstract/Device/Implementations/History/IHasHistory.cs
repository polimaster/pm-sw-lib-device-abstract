using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Identifies a device can return history
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
/// <typeparam name="TTransport">Data type for <see cref="ITransport{T}"/></typeparam>
public interface IHasHistory<THistory, TTransport> : IHasClock {
    
    /// <summary>
    /// Interval between history entries
    /// </summary>
    IDeviceSetting<ushort?> HistoryInterval { get; }

    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WipeHistory(CancellationToken cancellationToken = new());
    
    /// <summary>
    /// <see cref="IHistoryReader{THistory,TTransport}"/>
    /// </summary>
    IHistoryReader<THistory, TTransport> HistoryReader { get; set; }
}