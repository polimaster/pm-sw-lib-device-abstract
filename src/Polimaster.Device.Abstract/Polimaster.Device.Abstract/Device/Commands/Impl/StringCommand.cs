using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands.Impl; 

/// <summary>
/// String <see cref="ACommand{T}"/> implementation
/// </summary>
public abstract class StringCommand : ACommand<string> {
    /// <inheritdoc />
    protected StringCommand(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}