using System;

namespace Polimaster.Device.Abstract.Device.Commands.Exceptions;

/// <summary>
/// Rises while command and/or its parameters <see cref="WriteCommand{TValue,TCommand}.Validate"/> fails..
/// </summary>
public class CommandValidationException : Exception {
    /// <inheritdoc />
    public CommandValidationException(Exception exception) : base("Error while validating command", exception) {
    }
}