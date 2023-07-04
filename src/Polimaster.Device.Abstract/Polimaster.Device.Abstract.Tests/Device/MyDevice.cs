using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Tests.Device;



public class MyDevice : ADevice<string> {
    public IDeviceSetting<string>? TestSetting { get; set; }
    public IDeviceSetting<string>? EmptyTestSetting { get; set; }

    public override Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new()) {
        return Task.FromResult(new DeviceInfo());
    }

    public override void BuildSettings() {
    }
}