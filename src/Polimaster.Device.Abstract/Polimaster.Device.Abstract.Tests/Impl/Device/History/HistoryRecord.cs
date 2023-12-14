using System;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.History;

public class HistoryRecord {
    public DateTime Time { get; set; }
    public double DoseRate { get; set; }
    public double Dose { get; set; }
}