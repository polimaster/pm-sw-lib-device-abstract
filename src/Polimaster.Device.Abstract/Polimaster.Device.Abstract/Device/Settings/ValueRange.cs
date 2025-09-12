namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Numeric value range
/// </summary>
public struct ValueRange {
    /// <summary>
    /// Minimum value
    /// </summary>
    public double Min { get; set; }
    /// <summary>
    /// Maximum value
    /// </summary>
    public double Max { get; set; }
    /// <summary>
    /// Value step
    /// </summary>
    public double Step { get; set; }
}