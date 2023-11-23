using System;

namespace Polimaster.Device.Abstract.Device.Settings; 

/// <summary>
/// Exception while validating device setting
/// </summary>
public class SettingValidationResult : Exception {
    /// <inheritdoc />
    public SettingValidationResult(Exception exception) : base("Error while validating value", exception) {
    }

    /// <inheritdoc />
    public SettingValidationResult(string message) : base(message) {
    }
}