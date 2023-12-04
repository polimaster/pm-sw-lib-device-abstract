using System;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MemoryStreamParams : IFormattable {
    public int Capacity { get; set; }

    public override string ToString() => ToString(null, System.Globalization.CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) => $"Capacity:{Capacity}";
}