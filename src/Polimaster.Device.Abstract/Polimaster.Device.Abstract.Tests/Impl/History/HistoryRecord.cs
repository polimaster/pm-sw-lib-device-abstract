using Polimaster.Device.Abstract.Device.Implementations.History;

namespace Polimaster.Device.Abstract.Tests.Impl.History;

public class HistoryRecord : AHistoryRecord {
    public double DoseRate { get; set; }
    public double Dose { get; set; }
}