using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl; 

/// <summary>
/// String data writer
/// </summary>
public abstract class StringWriter : ADataWriter<string> {
    /// <inheritdoc />
    protected StringWriter(ITransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }

    /// <inheritdoc />
    protected override byte[] Compile(string data) => Encoding.UTF8.GetBytes(data);
}
/// <summary>
/// String data writer with verifying result returned from device
/// </summary>
public abstract class StringWriterVerified : ADataWriterVerified<string> {
    /// <inheritdoc />
    protected StringWriterVerified(ITransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }

    /// <inheritdoc />
    protected override byte[] Compile(string data) => Encoding.UTF8.GetBytes(data);
}