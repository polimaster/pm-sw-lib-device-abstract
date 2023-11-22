namespace Polimaster.Device.Abstract.Tests.Impl.Device.Transport; 

public class MemoryStreamParams {
    public int Capacity { get; set; }

    public override string ToString() {
        return $"Capacity:{Capacity}";
    }
}