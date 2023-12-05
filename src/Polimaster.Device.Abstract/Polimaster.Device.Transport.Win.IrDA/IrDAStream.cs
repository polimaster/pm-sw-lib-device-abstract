using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.IrDA;

/// <summary>
/// IrDA stream implementation
/// </summary>
public class IrDAStream : IDeviceStream<byte[]> {
    /// <summary>
    /// Max data length while reading transport stream
    /// </summary>
    private const int BUFF_LENGTH = 10000;
    private readonly ILogger<IrDAStream>? _logger;
    private readonly NetworkStream _stream;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="loggerFactory"></param>
    public IrDAStream(NetworkStream stream, ILoggerFactory? loggerFactory = null) {
        _stream = stream;
        _logger = loggerFactory?.CreateLogger<IrDAStream>();
    }

    /// <inheritdoc />
    public async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V} bytes", nameof(WriteAsync), buffer.Length);
        await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        
        var buff = new byte[BUFF_LENGTH];
        var len = await _stream.ReadAsync(buff, 0, BUFF_LENGTH, cancellationToken);

        var data = new byte[len];
        for (var i = 0; i < len; i++) data[i] = buff[i];
        return data;
    }
}