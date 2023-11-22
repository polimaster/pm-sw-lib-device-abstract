using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <inheritdoc />
public class TcpStream : IDeviceStream<string> {
    private readonly NetworkStream _networkStream;
    private readonly ILogger<TcpStream>? _logger;
    /// <summary>
    /// Max data length while reading transport stream
    /// </summary>
    private const int MAX_DATA_LENGTH = 10000;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="networkStream"></param>
    /// <param name="loggerFactory"></param>
    public TcpStream(NetworkStream networkStream, ILoggerFactory? loggerFactory) {
        _networkStream = networkStream;
        _logger = loggerFactory?.CreateLogger<TcpStream>();
    }

    /// <inheritdoc />
    public async Task WriteAsync(string buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteAsync), buffer);
        var v = Encoding.UTF8.GetBytes(buffer);
        await _networkStream.WriteAsync(v, 0, buffer.Length, cancellationToken);
        await _networkStream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        var arr = await ReadBytesAsync(cancellationToken);
        return Encoding.UTF8.GetString(arr);
    }

    private async Task<byte[]> ReadBytesAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        
        var buff = new byte[MAX_DATA_LENGTH];
        var len = await _networkStream.ReadAsync(buff, 0, MAX_DATA_LENGTH, cancellationToken);
    
        var data = new byte[len];
        for (var i = 0; i < len; i++) data[i] = buff[i];
        return data;
    }
}