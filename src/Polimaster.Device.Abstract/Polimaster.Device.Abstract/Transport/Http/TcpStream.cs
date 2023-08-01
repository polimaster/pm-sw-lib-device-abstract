using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <inheritdoc />
public class TcpStream : IDeviceStream {
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
    public TcpStream(NetworkStream networkStream, ILoggerFactory? loggerFactory = null) {
        _networkStream = networkStream;
        _logger = loggerFactory?.CreateLogger<TcpStream>();
    }

    /// <inheritdoc />
    public Stream BaseStream => _networkStream;

    /// <inheritdoc />
    public async Task WriteLineAsync(string value, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteLineAsync), value);
        var v = Encoding.UTF8.GetBytes(value);
        await WriteAsync(v, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> ReadLineAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadLineAsync));
        var arr = await ReadAsync(cancellationToken);
        return Encoding.UTF8.GetString(arr);
    }

    /// <inheritdoc />
    public async Task WriteAsync(byte[] buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V} bytes", nameof(WriteAsync), buffer.Length);
        await BaseStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
        await BaseStream.FlushAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        
        var buff = new byte[MAX_DATA_LENGTH];
        var len = await BaseStream.ReadAsync(buff, 0, MAX_DATA_LENGTH, cancellationToken);

        var data = new byte[len];
        for (var i = 0; i < len; i++) data[i] = buff[i];
        return data;
    }
}