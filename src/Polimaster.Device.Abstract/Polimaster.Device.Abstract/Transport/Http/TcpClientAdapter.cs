using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <inheritdoc cref="Polimaster.Device.Abstract.Transport.IClient{TConnectionParams}" />
public class TcpClientAdapter : AClient<string, HttpConnectionParams> {
    private readonly TcpClient _wrapped;
    
    /// <summary>
    /// 
    /// </summary>
    public TcpClientAdapter(HttpConnectionParams connectionParams, ILoggerFactory? loggerFactory) : base(connectionParams, loggerFactory) {
        _wrapped = new TcpClient();
    }

    /// <inheritdoc />
    public override bool Connected => _wrapped.Connected;

    /// <inheritdoc />
    public override void Close() {
        _wrapped.Close();
    }

    /// <inheritdoc />
    public override IDeviceStream<string> GetStream() => new TcpStream(_wrapped.GetStream(), LoggerFactory);

    /// <inheritdoc />
    public override void Open() {
        _wrapped.Connect(ConnectionParams.Ip, ConnectionParams.Port);
    }

    /// <inheritdoc />
    public override async Task OpenAsync() {
        await _wrapped.ConnectAsync(ConnectionParams.Ip, ConnectionParams.Port);
    }

    /// <inheritdoc />
    public override void Dispose() {
        _wrapped.Dispose();
    }
}