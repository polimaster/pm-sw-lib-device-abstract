using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings; 

public class MyDeviceSetting : DeviceSettingBase<MyParam> {
    public MyDeviceSetting(ITransport transport, ICommand<MyParam> readCommand, ICommand<MyParam>? writeCommand = null) : base(transport, readCommand, writeCommand) {
    }
}