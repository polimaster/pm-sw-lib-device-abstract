using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Tests.Impl.Device.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device;

public interface IMyDevice {
    IDeviceSetting<MyParam> MyParamSetting { get; }
}

public class MyDevice : ADevice, IMyDevice {
    public IDeviceSetting<MyParam> MyParamSetting { get; }
    public IDeviceSetting<string>? StringSetting { get; set; }

    public MyDevice(ITransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
        
        // building device commands and settings
        var testSettingsReadCommand = new MyParamReader(loggerFactory);
        var testSettingsWriteCommand = new MyParamWriter(loggerFactory);
        MyParamSetting = SettingBuilder.
            WithReadCommand(testSettingsReadCommand).
            WithWriteCommand(testSettingsWriteCommand).
            Build<MyParam>();
    }
    

    public override async Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken = new()) {
        throw new System.NotImplementedException();
    }
}