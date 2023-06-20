using System;

namespace Polimaster.Device.Abstract.Commands;

/// <summary>
/// Rises while command and/or its parameters validation fails..
/// </summary>
public class CommandValidationException : Exception {
    public CommandValidationException(Exception exception) : base("Error while validating command", exception) {
    }
}