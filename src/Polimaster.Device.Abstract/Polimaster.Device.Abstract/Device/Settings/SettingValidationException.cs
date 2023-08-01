using System;

namespace Polimaster.Device.Abstract.Device.Settings; 

/// <summary>
/// Exception while validating device setting
/// </summary>
public class SettingValidationException : Exception {
    /// <inheritdoc />
    public SettingValidationException(Exception exception) : base("Error while validating value", exception) {
    }

    /// <inheritdoc />
    public SettingValidationException(string message) : base(message) {
    }
}