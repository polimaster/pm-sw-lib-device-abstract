using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// Byte array data reader
/// </summary>
/// <typeparam name="T">Type of data to read</typeparam>
public abstract class ByteReader<T> : ADataReader<T, byte[]> {
    /// <inheritdoc />
    protected ByteReader(ITransport<byte[]> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}