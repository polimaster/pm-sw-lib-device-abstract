using System;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Rises while result of <see cref="ACommand{TValue,TTransportData}.Parse"/> fails
/// </summary>
public class CommandResultParsingException : Exception {
    /// <inheritdoc />
    public CommandResultParsingException(Exception exception) : base("Error while parsing result of command", exception) {
    }

    /// <inheritdoc />
    public CommandResultParsingException(string message) : base(message) {
    }
}