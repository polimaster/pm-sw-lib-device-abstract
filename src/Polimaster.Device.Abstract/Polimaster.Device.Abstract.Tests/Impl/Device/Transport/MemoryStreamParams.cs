namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MemoryStreamParams : IStringify {
    public int Capacity { get; set; }

    public override string ToString() => $"Capacity:{Capacity}";
}