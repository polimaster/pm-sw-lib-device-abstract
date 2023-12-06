using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Stream;

/// <summary>
/// Device byte stream implementation
/// </summary>
public class NetworkByteStream : IDeviceStream<byte[]> {
    private readonly ILogger<NetworkByteStream>? _logger;
    private readonly NetworkStream _stream;
    
    /// <summary>
    /// Buffer length while reading stream
    /// </summary>
    public int BuffLength { get; set; } = 256;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="loggerFactory"></param>
    public NetworkByteStream(NetworkStream stream, ILoggerFactory? loggerFactory = null) {
        _stream = stream;
        _logger = loggerFactory?.CreateLogger<NetworkByteStream>();
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V} bytes", nameof(WriteAsync), buffer.Length);
        await _stream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));

        var result = new List<byte>();
        var buffer = new byte[BuffLength];
        do {
            var bytes = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            if (bytes == 0) continue;

            result.AddRange(buffer);
        } while (_stream.DataAvailable);

        return result.ToArray();
    }
}