using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device; 

public interface IHasBattery {
    
    BatteryStatus BatteryStatus { get; protected set; }
    
    Task<BatteryStatus> RefreshBatteryStatus();
}

public struct BatteryStatus {
    public double? Volts;
    public double? Percents;
}