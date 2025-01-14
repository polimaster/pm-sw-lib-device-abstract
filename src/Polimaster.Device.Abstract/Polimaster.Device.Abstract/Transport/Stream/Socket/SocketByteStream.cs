using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Stream.Socket;

/// <summary>
/// Device byte stream implementation
/// </summary>
public class SocketByteStream : IDeviceStream {
    /// <summary>
    /// See <see cref="ILogger"/>
    /// </summary>
    private readonly ILogger? _logger;

    /// <summary>
    /// Underlying stream
    /// </summary>
    protected readonly ISocketStream Stream;
    
    /// <summary>
    /// Buffer length while reading stream
    /// </summary>
    public virtual int BuffLength { get; set; } = 256;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="loggerFactory"></param>
    public SocketByteStream(ISocketStream stream, ILoggerFactory? loggerFactory = null) {
        Stream = stream;
        _logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync<T>(byte[] data, CancellationToken cancellationToken, T? channel = default) {
        _logger?.LogDebug("Call {F} with: {V} bytes", nameof(WriteAsync), data.Length);
        await Stream.WriteAsync(data, 0, data.Length, cancellationToken);
        await Stream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<byte[]> ReadAsync<T>(CancellationToken cancellationToken, T? channel = default) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));

        var result = new List<byte>();
        var buffer = new byte[BuffLength];
        do {
            var bytes = await Stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            if (bytes == 0) continue;
        
            result.AddRange(buffer);
        } while (Stream.DataAvailable);
        
        return result.ToArray();
    }
}