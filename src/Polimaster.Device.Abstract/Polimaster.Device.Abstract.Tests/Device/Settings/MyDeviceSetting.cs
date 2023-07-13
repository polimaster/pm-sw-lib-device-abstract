using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Tests.Device.Settings; 

public class MyDeviceSetting : DeviceSettingBase<MyParam> {
    
}

public class MyDeviceSettingValidatable : MyDeviceSetting {
    
    /// <summary>
    /// Validates value.
    /// Sets <see cref="IDeviceSetting{T}.ValidationErrors"/> if length of value greater than 10.
    /// </summary>
    /// <param name="value"></param>
    protected override void Validate(MyParam? value) {
        if (value?.Value.Length > 0) {
            ValidationErrors = new[] { new SettingValidationException("Value greater than 10") };
            return;
        }

        ValidationErrors = null;
    }
}

public class MyDeviceSettingProxy : ADeviceSettingProxy<string, string> {
    public override string? FromProxied(string? value) {
        return value;
    }

    public override string? ToProxied(string? value) {
        return value;
    }
}