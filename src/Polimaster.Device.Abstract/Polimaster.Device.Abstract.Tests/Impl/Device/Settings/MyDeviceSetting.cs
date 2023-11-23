using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Device.Settings; 

public class MyDeviceSetting : DeviceSettingBase<MyParam> {
    public MyDeviceSetting(ITransport transport, IDataReader<MyParam> reader, IDataWriter<MyParam>? writer = null) : base(transport, reader, writer) {
    }
    
    protected override void Validate(MyParam? value) {
        if (value == null) {
            ValidationErrors = new[] { new SettingValidationResult("Value is null") };
            return;
        }
        
        var vc = new ValidationContext(value);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(value, vc, results, true);

        if (isValid) {
            ValidationErrors = null;
            return;
        }

        ValidationErrors = results.Select(e => new SettingValidationResult(e.ErrorMessage ?? "Unknown error"));
    }
}