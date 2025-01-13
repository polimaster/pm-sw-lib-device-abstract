using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// Byte array data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public abstract class ByteWriter<T> : ADataWriter<T, byte[]> {
    /// <inheritdoc />
    protected ByteWriter(ITransport<byte[]> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}

/// <summary>
/// Byte array data writer with verifying result returned from device
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public abstract class ByteWriterVerified<T> : ADataWriterVerified<T, byte[]> {
    /// <inheritdoc />
    protected ByteWriterVerified(ITransport<byte[]> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}