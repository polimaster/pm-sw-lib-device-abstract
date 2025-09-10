using System.ComponentModel.DataAnnotations;
using System.Linq;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class MyParamSettingProxy(IDeviceSetting<MyParam> proxiedSetting, ISettingDescriptor settingDescriptor)
    : ADeviceSettingProxy<string, MyParam>(proxiedSetting, settingDescriptor) {
    public static readonly string[] FORBIDDEN_VALUES = ["string1", "string2"];

    protected override string? GetProxied() {
        return ProxiedSetting.Value?.Value;
    }

    protected override MyParam ModifyProxied(MyParam proxied, string value) {
        proxied.Value = value;
        return proxied;
    }

    protected override void Validate(string? value) {
        base.Validate(value);

        if (FORBIDDEN_VALUES.Contains(value))
            ValidationResults.Add(new ValidationResult($"{value} cant be set as value"));
    }
}