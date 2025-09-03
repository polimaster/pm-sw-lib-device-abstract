using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Tests.Impl.Commands;
using Polimaster.Device.Abstract.Tests.Impl.Transport;
using ValidationResult = Polimaster.Device.Abstract.Device.Settings.ValidationResult;

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


    protected override void Validate(MyParam? value) {
        base.Validate(value);

        if (value == null) return;
        var vc = new ValidationContext(value);
        var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var isValid = Validator.TryValidateObject(value, vc, results, true);

        if (!isValid) {
            ValidationErrors.AddRange(results.Select(e => new ValidationResult(e.ErrorMessage ?? "Unknown error")));
        }
    }
}