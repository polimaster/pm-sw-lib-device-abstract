using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

public class MyDeviceSettingProxy : ADeviceSettingProxy<string, string> {
    protected override string? FromProxied(string? value) {
        return value;
    }

    protected override string? ToProxied(string? value) {
        return value;
    }

    public MyDeviceSettingProxy(IDeviceSetting<string> proxiedSetting) : base(proxiedSetting) {
    }
}