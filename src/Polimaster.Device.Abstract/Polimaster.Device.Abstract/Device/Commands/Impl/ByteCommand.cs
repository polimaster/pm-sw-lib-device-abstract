using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;


/// <summary>
/// Byte array <see cref="ACommand{T}"/> implementation
/// </summary>
public abstract class ByteCommand : ACommand<byte[]>{
    /// <inheritdoc />
    protected ByteCommand(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}