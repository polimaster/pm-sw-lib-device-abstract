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

/// <summary>
///Byte array <see cref="ACommand{T}"/> implementation with verifying result returned from device
/// </summary>
public abstract class ByteCommandVerified : ACommandVerified<byte[]>{
    /// <inheritdoc />
    protected ByteCommandVerified(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}