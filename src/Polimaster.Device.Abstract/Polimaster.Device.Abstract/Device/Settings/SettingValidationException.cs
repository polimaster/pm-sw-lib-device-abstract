using System;

namespace Polimaster.Device.Abstract.Device.Settings; 

public class SettingValidationException : Exception {
    public SettingValidationException(Exception exception) : base("Error while validating value", exception) {
    }

    public SettingValidationException(string message) : base(message) {
    }
}