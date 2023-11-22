using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings;

public class MyParamSettingValidatable : DeviceSettingBase<MyParam> {
    
    /// <summary>
    /// Validates value.
    /// Sets <see cref="IDeviceSetting{T}.ValidationErrors"/> if length of value greater than 10.
    /// </summary>
    /// <param name="value"></param>
    protected override void Validate(MyParam? value) {
        if (value?.Value?.Length > 0) {
            ValidationErrors = new[] { new SettingValidationException("Value greater than 10") };
            return;
        }

        ValidationErrors = null;
    }

    public MyParamSettingValidatable(ITransport transport, IDataReader<MyParam> readCommand, IDataWriter<MyParam>? writeCommand = null) : base(transport, readCommand, writeCommand) {
    }
}