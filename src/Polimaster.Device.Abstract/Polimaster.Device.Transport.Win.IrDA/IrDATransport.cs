using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <inheritdoc />
public class IrDATransport : ATransport<byte[]> {
    /// <inheritdoc />
    public IrDATransport(IClient<byte[]> client, ILoggerFactory? loggerFactory) : base(client, loggerFactory) {
    }
}