using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Stream; 

/// <summary>
/// Device string stream implementation
/// </summary>
public class NetworkStringStream : IDeviceStream<string> {
    private readonly NetworkByteStream _stream;
    
    /// <summary>
    /// Buffer length while reading stream
    /// </summary>
    public int BuffLength {
        get => _stream.BuffLength;
        set => _stream.BuffLength = value;
    }

    /// <summary>
    /// String <see cref="Encoding"/>
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.UTF8;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="loggerFactory"></param>
    public NetworkStringStream(NetworkStream stream, ILoggerFactory? loggerFactory = null) {
        _stream = new NetworkByteStream(stream, loggerFactory);
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync(string buffer, CancellationToken cancellationToken) {
        var v = Encoding.GetBytes(buffer);
        await _stream.WriteAsync(v, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<string> ReadAsync(CancellationToken cancellationToken) {
        var res = await _stream.ReadAsync(cancellationToken);
        return Encoding.GetString(res);
    }
}