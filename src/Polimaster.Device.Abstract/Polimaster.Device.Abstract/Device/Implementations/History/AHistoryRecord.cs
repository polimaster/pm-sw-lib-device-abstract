using System;

namespace Polimaster.Device.Abstract.Device.Implementations.History;

/// <summary>
/// Device history record
/// </summary>
public abstract class AHistoryRecord {
    /// <summary>
    /// Record date/time
    /// </summary>
    public DateTimeOffset Time { get; set; }
}