using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// Byte array data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public abstract class ByteWriter<T> : ADataWriter<T, byte[]> {
    /// <inheritdoc />
    protected ByteWriter(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}

/// <summary>
/// Byte array data writer with verifying result returned from device
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public abstract class ByteWriterVerified<T> : ADataWriterVerified<T, byte[]> {
    /// <inheritdoc />
    protected ByteWriterVerified(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}