using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using ValidationResult = Polimaster.Device.Abstract.Device.Settings.ValidationResult;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings; 

public class MyParamSetting : DeviceSettingBase<MyParam> {
    public MyParamSetting(IDataReader<MyParam> reader, IDataWriter<MyParam>? writer = null) : base(reader, writer) {
    }
    
    protected override void Validate(MyParam? value) {
        base.Validate(value);
        
        if (value == null) {
            ValidationErrors = new[] { new ValidationResult("Value is null") };
            return;
        }
        
        var vc = new ValidationContext(value);
        var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
        var isValid = Validator.TryValidateObject(value, vc, results, true);

        if (!isValid) {
            ValidationErrors = results.Select(e => new ValidationResult(e.ErrorMessage ?? "Unknown error"));
        }
        
    }
}