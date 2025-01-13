using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;


/// <summary>
/// Byte array <see cref="ACommand{T}"/> implementation
/// </summary>
public abstract class ByteCommand : ACommand<byte[]>{
    /// <inheritdoc />
    protected ByteCommand(ITransport<byte[]> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}

/// <summary>
///Byte array <see cref="ACommand{T}"/> implementation with verifying result returned from device
/// </summary>
public abstract class ByteCommandVerified : ACommandVerified<byte[]>{
    /// <inheritdoc />
    protected ByteCommandVerified(ITransport<byte[]> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}