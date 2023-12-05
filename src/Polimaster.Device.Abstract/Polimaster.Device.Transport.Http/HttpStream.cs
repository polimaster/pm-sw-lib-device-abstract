using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc />
public class HttpStream : IDeviceStream<string> {
    private readonly NetworkStream _stream;
    private readonly ILogger<HttpStream>? _logger;
    /// <summary>
    /// Max data length while reading transport stream
    /// </summary>
    private const int BUFF_LENGTH = 256;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="loggerFactory"></param>
    public HttpStream(NetworkStream stream, ILoggerFactory? loggerFactory) {
        _stream = stream;
        _logger = loggerFactory?.CreateLogger<HttpStream>();
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync(string buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteAsync), buffer);
        var v = Encoding.UTF8.GetBytes(buffer);
        await _stream.WriteAsync(v, 0, buffer.Length, cancellationToken);
        await _stream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<string> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        var response = new StringBuilder();
        
        var buffer = new byte[BUFF_LENGTH];
        do {
            var bytes = await _stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
            if (bytes == 0) continue;

            var s = Encoding.UTF8.GetString(buffer, 0, bytes);
            response.Append(s);
        } while (_stream.DataAvailable);

        return response.ToString();
    }
    
}