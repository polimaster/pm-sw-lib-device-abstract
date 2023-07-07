using System.Collections.Generic;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

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

    public HistoryChunk() {
        Remaining = null;
        Completed = false;
    }
}