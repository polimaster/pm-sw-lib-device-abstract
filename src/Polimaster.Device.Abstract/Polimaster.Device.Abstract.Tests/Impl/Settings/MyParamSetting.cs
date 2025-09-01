using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using ValidationResult = Polimaster.Device.Abstract.Device.Settings.ValidationResult;

namespace Polimaster.Device.Abstract.Tests.Impl.Settings; 

public class MyParamSetting(IDataReader<MyParam> reader, ISettingDescriptor settingDescriptor, IDataWriter<MyParam>? writer = null)
    : ADeviceSetting<MyParam>(reader, settingDescriptor, writer) {
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