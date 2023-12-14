using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream.Socket;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <summary>
/// IrDA stream implementation
/// </summary>
public class IrDAStream : SocketByteStream {
    /// <inheritdoc />
    public IrDAStream(ISocketStream stream, ILoggerFactory? loggerFactory = null) : base(stream, loggerFactory) {
    }
}