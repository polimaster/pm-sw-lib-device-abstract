using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyDevice : ADevice<string> {
    public MyDevice(ITransport<string> transport) : base(transport) {
    }

    public override Task<DeviceInfo> ReadDeviceInfo() {
        return Task.FromResult(new DeviceInfo());
    }
}