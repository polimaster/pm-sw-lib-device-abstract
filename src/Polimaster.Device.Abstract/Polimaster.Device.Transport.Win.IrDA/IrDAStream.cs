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

    // public override async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
    //     var buff = new byte[10000];
    //     var len = await _stream.ReadAsync(buff, 0, BUFF_LENGTH, cancellationToken);
    //     
    //     var data = new byte[len];
    //     for (var i = 0; i < len; i++) data[i] = buff[i];
    //     return data;
    // }
}