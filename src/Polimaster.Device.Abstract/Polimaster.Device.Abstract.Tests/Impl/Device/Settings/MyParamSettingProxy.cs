using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

public class MyParamSettingProxy : ADeviceSettingProxy<string, MyParam> {
    protected override string? FromProxied(MyParam? value) {
        return string.Empty;
    }

    protected override MyParam? ToProxied(string? value) {
        return new MyParam();
    }


    public MyParamSettingProxy(IDeviceSetting<MyParam> proxiedSetting) : base(proxiedSetting) {
    }
}