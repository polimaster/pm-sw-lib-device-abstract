using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands.Impl; 

/// <summary>
/// String data writer
/// </summary>
/// <typeparam name="T">Type of data to write</typeparam>
public abstract class StringWriter<T> : ADataWriter<T, string> {
    /// <inheritdoc />
    protected StringWriter(ILoggerFactory? loggerFactory) : base(loggerFactory) {
    }
}