using System;

namespace Polimaster.Device.Abstract.Device.Commands.Exceptions; 

/// <summary>
/// Rises while result of <see cref="ADataWriter{T,TSteamData}.Compile"/> fails
/// </summary>
public class CommandCompilationException : Exception{
    /// <inheritdoc />
    public CommandCompilationException(string message) : base(message) {
    }
}