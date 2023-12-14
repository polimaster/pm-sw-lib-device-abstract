using System.Threading;
using System.Threading.Tasks;
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

    /// <inheritdoc />
    public override int BuffLength { get; set; } = 10000;

    /// <inheritdoc />
    public override async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        var buff = new byte[BuffLength];
        var len = await Stream.ReadAsync(buff, 0, BuffLength, cancellationToken);
        
        var data = new byte[len];
        for (var i = 0; i < len; i++) data[i] = buff[i];
        return data;
    }
}