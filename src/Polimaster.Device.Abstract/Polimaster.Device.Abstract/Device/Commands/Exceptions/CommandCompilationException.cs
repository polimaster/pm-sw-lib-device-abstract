using System;

namespace Polimaster.Device.Abstract.Device.Commands.Exceptions;

/// <summary>
/// Rises while result of <see cref="ADataWriter{TValue, TData, TStream}.Compile"/> fails
/// </summary>
public class CommandCompilationException : Exception {
    /// <inheritdoc />
    public CommandCompilationException(string message) : base(message) {
    }

    /// <inheritdoc />
    public CommandCompilationException(Exception exception) : base("Error while compiling command", exception) {
    }
}