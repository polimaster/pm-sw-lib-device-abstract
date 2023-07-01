using System;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Rises while command and/or its parameters <see cref="AWriteCommand{T,TTransport}.Validate"/> fails..
/// </summary>
public class CommandValidationException : Exception {
    public CommandValidationException(Exception exception) : base("Error while validating command", exception) {
    }
}