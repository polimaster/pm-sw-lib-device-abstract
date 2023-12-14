using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// Byte array data reader
/// </summary>
/// <typeparam name="T">Type of data to read</typeparam>
public abstract class ByteReader<T> : ADataReader<T, byte[]> {
    /// <inheritdoc />
    protected ByteReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}