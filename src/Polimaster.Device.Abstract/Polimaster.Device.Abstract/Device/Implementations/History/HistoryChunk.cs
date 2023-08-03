using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Part of history returned from device
/// </summary>
/// <typeparam name="THistory"></typeparam>
public struct HistoryChunk<THistory> {
    /// <summary>
    /// History records available in device
    /// </summary>
    public int? Remaining;

    /// <summary>
    /// Indicates if reading process completed
    /// </summary>
    public bool Completed;

    /// <summary>
    /// Retrieved history records
    /// </summary>
    public IEnumerable<THistory>? Records;

    /// <summary>
    /// 
    /// </summary>
    public HistoryChunk() {
        Remaining = null;
        Completed = false;
    }
}