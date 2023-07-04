using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Tests.Device.Settings; 

public class MyDeviceSetting : DeviceSettingBase<string> {
    
}

public class MyDeviceSettingValidatable : MyDeviceSetting {
    
    /// <summary>
    /// Validates value.
    /// Sets <see cref="IDeviceSetting{T}.ValidationErrors"/> if length of value greater than 10.
    /// </summary>
    /// <param name="value"></param>
    protected override void Validate(string? value) {
        if (value?.Length > 0) {
            ValidationErrors = new[] { new SettingValidationException("Value greater than 10") };
            return;
        }

        ValidationErrors = null;
    }
}