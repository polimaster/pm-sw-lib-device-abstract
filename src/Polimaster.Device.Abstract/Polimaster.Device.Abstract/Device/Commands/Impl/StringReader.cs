using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// String data writer
/// </summary>
/// <typeparam name="T">Type of data to read</typeparam>
public abstract class StringReader<T> : ADataReader<T, string> {
    /// <inheritdoc />
    protected StringReader(ITransport<string> transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }
}