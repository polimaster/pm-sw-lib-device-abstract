using System;
using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Part of history returned from the device
/// </summary>
/// <typeparam name="THistory"></typeparam>
public struct HistoryChunk<THistory> {
    /// <summary>
    /// History records available in the device
    /// </summary>
    public int? Remaining = null;

    /// <summary>
    /// Indicates if the reading process completed
    /// </summary>
    public bool Completed = false;

    /// <summary>
    /// Retrieved history records
    /// </summary>
    public IEnumerable<THistory>? Records = null;

    /// <summary>
    /// Stores <see cref="Exception"/> occured while reading history data from device.
    /// </summary>
    public Exception? Exception = null;

    /// <summary>
    ///
    /// </summary>
    public HistoryChunk() {
    }
}