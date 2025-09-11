using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings;

public class MyParamSetting : ADeviceSetting<MyParam> {
    public MyParamSetting(IMyTransport transport,
        IMySettingDescriptors settingDescriptor,
        ILoggerFactory? loggerFactory) : base(new SettingDefinition<MyParam> {
        Reader = new MyParamReader(transport, loggerFactory),
        Descriptor = settingDescriptor.MyParamSettingDescriptor,
        Writer = new MyParamWriter(transport, loggerFactory)
    }) {
    }

    public MyParamSetting(SettingDefinition<MyParam> settingDefinition) : base(settingDefinition) {
    }

    // protected override void Validate() {
    //     base.Validate();
    //
    //     if (Value == null) return;
    //     var vc = new ValidationContext(Value);
    //     Validator.TryValidateObject(value, vc, ValidationResults, true);
    // }
}