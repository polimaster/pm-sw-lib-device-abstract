using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Tests.Device;

public class MyDevice : ADevice<string> {
    public MyDevice(ITransport<string> transport) : base(transport, new CommandBuilder<string>(),
        new DeviceSettingBuilder<string>()) {
    }

    public override Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new()) {
        return Task.FromResult(new DeviceInfo());
    }
}