using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history reader
/// </summary>
/// <typeparam name="THistory">Type of history record</typeparam>
/// <typeparam name="TTransport">Data type for <see cref="ITransport{T}"/></typeparam>
public interface IHistoryReader<THistory, TTransport> {
    
    /// <summary>
    /// History records available in device
    /// </summary>
    int? Available { get; }
    
    /// <summary>
    /// Occurs when new data got from device
    /// </summary>
    Action<IEnumerable<THistory>> HasNext { get; set; }

    /// <summary>
    /// Device <see cref="IHistoryReader{THistory,TTransport}"/> belongs to
    /// </summary>
    IDevice<TTransport> Device { get; set; }

    /// <summary>
    /// Reads history from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Array of history records</returns>
    Task Read(CancellationToken cancellationToken = new());
}