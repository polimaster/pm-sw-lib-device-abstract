using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl; 

/// <summary>
/// String <see cref="ACommand{T}"/> implementation
/// </summary>
public abstract class StringCommand : ACommand<string> {
    /// <inheritdoc />
    protected StringCommand(ITransport<string> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}
/// <summary>
/// String <see cref="ACommand{T}"/> implementation with verifying result returned from device
/// </summary>
public abstract class StringCommandVerified : ACommandVerified<string> {
    /// <inheritdoc />
    protected StringCommandVerified(ITransport<string> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}