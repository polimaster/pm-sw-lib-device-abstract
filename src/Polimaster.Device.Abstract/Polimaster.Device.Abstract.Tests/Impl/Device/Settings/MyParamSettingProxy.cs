using System.Linq;
using Polimaster.Device.Abstract.Device.Settings;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

public class MyParamSettingProxy : ADeviceSettingProxy<string, MyParam> {
    public static readonly string[] FORBIDDEN_VALUES = { "string1", "string2" };

    protected override string? GetProxied() {
        return ProxiedSetting.Value?.Value;
    }

    protected override void SetProxied(string? value) {
        if (ProxiedSetting.Value == null) {
            ProxiedSetting.Value = new MyParam { Value = value };
        } else {
            ProxiedSetting.Value.Value = value;
        }
    }

    protected override void Validate(string? value) {
        base.Validate(value);
        
        if (value == null) {
            ValidationErrors = new[] { new ValidationResult("Value is null") };
            return;
        }

        if (FORBIDDEN_VALUES.Contains(value)) {
            ValidationErrors = new[] { new ValidationResult($"{value} cant be set as value") };
        }
    }

    public MyParamSettingProxy(IDeviceSetting<MyParam> proxiedSetting) : base(proxiedSetting) {
    }
}