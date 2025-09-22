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

    protected override MyParam CreateNewProxiedValue(MyParam proxied, string value) {
        return proxied with { Value = value };
    }

    protected override void Validate() {
        base.Validate();

        if (FORBIDDEN_VALUES.Contains(Value))
            ValidationResults.Add(new ValidationResult($"{Value} cant be set as value"));
    }
}