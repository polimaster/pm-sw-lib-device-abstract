using System;

namespace Polimaster.Device.Abstract.Device.Commands; 

/// <summary>
/// Rises while result of command compilation fails
/// </summary>
public class CommandCompilationException : Exception{
    public CommandCompilationException(string message) : base(message) {
    }
}