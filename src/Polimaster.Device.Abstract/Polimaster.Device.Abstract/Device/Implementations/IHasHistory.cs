using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Implementations;

/// <summary>
/// Identifies a device can return history
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
public interface IHasHistory<THistory> : IHasClock {
    
    /// <summary>
    /// Interval between history entries
    /// </summary>
    IDeviceSetting<ushort?> HistoryInterval { get; }
    
    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Array of history records</returns>
    Task<IEnumerable<THistory>?> ReadHistory(CancellationToken cancellationToken = new());

    /// <summary>
    /// Wipe history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WipeHistory(CancellationToken cancellationToken = new());
}

/// <summary>
/// Identifies a device can return history with parameters
/// </summary>
/// <typeparam name="T">Type of history record</typeparam>
/// <typeparam name="TParams">History reading parameters</typeparam>
public interface IHasHistory<T, in TParams> : IHasHistory<T> {
    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="parameters">Parameters while reading history</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Array of history records</returns>
    Task<IEnumerable<T>> ReadHistory(TParams? parameters = default, CancellationToken cancellationToken = new());
}