using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// String data writer
/// </summary>
/// <typeparam name="T">Type of data to read</typeparam>
public abstract class StringReader<T> : ADataReader<T, string> {
    /// <inheritdoc />
    protected StringReader(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}