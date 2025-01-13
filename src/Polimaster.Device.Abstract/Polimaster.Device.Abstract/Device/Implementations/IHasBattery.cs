using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implementations; 

/// <summary>
/// Identifies a device with battery
/// </summary>
public interface IHasBattery {
    
    /// <summary>
    /// Status of device battery
    /// </summary>
    BatteryStatus? BatteryStatus { get; }

    /// <summary>
    /// Read battery status from device
    /// </summary>
    /// <param name="token">The token to monitor for cancellation requests.</param>
    /// <returns><see cref="BatteryStatus"/></returns>
    Task<BatteryStatus?> RefreshBatteryStatus(CancellationToken token);
}

/// <summary>
/// Status of device battery
/// </summary>
public struct BatteryStatus {
    
    /// <summary>
    /// Value in Volts
    /// </summary>
    public double? Volts;
    
    /// <summary>
    /// Value in percents
    /// </summary>
    public double? Percents;
}