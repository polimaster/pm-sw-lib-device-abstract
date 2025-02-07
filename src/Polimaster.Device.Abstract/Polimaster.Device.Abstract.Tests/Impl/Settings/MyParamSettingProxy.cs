using System.Linq;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class MyParamSettingProxy(IDeviceSetting<MyParam> proxiedSetting, ISettingBehaviour? settingBehaviour = null)
    : ADeviceSettingProxy<string, MyParam>(proxiedSetting, settingBehaviour) {
    public static readonly string[] FORBIDDEN_VALUES = ["string1", "string2"];

    protected override string? GetProxied() {
        return ProxiedSetting.Value?.Value;
    }

    protected override MyParam SetProxied(MyParam proxied, string value) {
        proxied.Value = value;
        return proxied;
    }

    protected override void Validate(string? value) {
        base.Validate(value);

        if (FORBIDDEN_VALUES.Contains(value))
            ValidationErrors.Add(new ValidationResult($"{value} cant be set as value"));
    }
}