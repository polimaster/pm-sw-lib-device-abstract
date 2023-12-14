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
/// <summary>
/// String <see cref="ACommand{T}"/> implementation with verifying result returned from device
/// </summary>
public abstract class StringCommandVerified : ACommandVerified<string> {
    /// <inheritdoc />
    protected StringCommandVerified(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}