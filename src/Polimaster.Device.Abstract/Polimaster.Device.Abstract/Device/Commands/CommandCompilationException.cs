using System;

namespace Polimaster.Device.Abstract.Device.Commands; 

/// <summary>
/// Rises while result of <see cref="ACommand{T,TTransport}.Compile"/> fails
/// </summary>
public class CommandCompilationException : Exception{
    /// <inheritdoc />
    public CommandCompilationException(string message) : base(message) {
    }
}