using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyDevice : ADevice<string> {
    public MyDevice() {
    }

    public override Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new()) {
        return Task.FromResult(new DeviceInfo());
    }

    public override void BuildSettings() {
    }
}