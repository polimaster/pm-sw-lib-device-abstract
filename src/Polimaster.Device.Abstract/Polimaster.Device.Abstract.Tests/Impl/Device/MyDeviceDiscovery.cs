using System.Threading;

namespace Polimaster.Device.Abstract.Tests.Impl.Device; 

public class MyDeviceDiscovery : ATransportDiscovery {
    public override void Start(CancellationToken token) {
        throw new System.NotImplementedException();
    }

    public override void Stop() {
        throw new System.NotImplementedException();
    }

    public override void Dispose() {
        throw new System.NotImplementedException();
    }
}