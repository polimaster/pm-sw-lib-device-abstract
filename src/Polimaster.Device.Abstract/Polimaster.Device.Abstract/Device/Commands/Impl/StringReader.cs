using System.Text;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands.Impl;

/// <summary>
/// String data writer
/// </summary>
public abstract class StringReader : ADataReader<string> {
    /// <inheritdoc />
    protected StringReader(ITransport transport, ILoggerFactory? loggerFactory) : base(transport, loggerFactory) {
    }

    /// <inheritdoc />
    protected override string Parse(byte[] res) => Encoding.UTF8.GetString(res);
}