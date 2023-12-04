using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc />
public class TcpClientAdapter : AClient<string, EndPoint> {
    private readonly TcpClient _wrapped;

    /// <inheritdoc />
    public TcpClientAdapter(EndPoint iPEndPoint, ILoggerFactory? loggerFactory) : base(iPEndPoint, loggerFactory) {
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
        _wrapped.Connect(Params);
    }

    /// <param name="token"></param>
    /// <inheritdoc />
    public override async Task OpenAsync(CancellationToken token) {
        await _wrapped.ConnectAsync(Params.Address, Params.Port);
    }

    /// <inheritdoc />
    public override void Dispose() {
        _wrapped.Dispose();
    }
}