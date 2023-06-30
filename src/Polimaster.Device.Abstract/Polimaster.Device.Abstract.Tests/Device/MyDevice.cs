using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyDevice : ADevice<string> {
    public MyDevice(ITransport<string> transport) : base(transport, new CommandBuilder(), new DeviceSettingBuilder()) {
    }

    public override Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new()) {
        return Task.FromResult(new DeviceInfo());
    }
}