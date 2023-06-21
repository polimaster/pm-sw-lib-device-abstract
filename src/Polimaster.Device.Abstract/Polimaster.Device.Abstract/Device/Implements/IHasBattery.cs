using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Implements; 

public interface IHasBattery {
    
    BatteryStatus BatteryStatus { get; }
    
    Task<BatteryStatus> RefreshBatteryStatus();
}

public struct BatteryStatus {
    public double? Volts;
    public double? Percents;
}