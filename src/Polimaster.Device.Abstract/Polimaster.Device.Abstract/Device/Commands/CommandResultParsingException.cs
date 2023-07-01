using System;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Rises while result of command parsing fails
/// </summary>
public class CommandResultParsingException : Exception {
    public CommandResultParsingException(Exception exception) : base("Error while parsing result of command", exception) {
    }

    public CommandResultParsingException(string message) : base(message) {
    }
}