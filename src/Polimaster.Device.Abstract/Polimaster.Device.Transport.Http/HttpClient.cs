using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;
using Polimaster.Device.Abstract.Transport.Stream.Socket;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc />
public class HttpClient : AClient<string, EndPoint> {
    private TcpClient? _wrapped;

    /// <inheritdoc />
    public HttpClient(EndPoint iPEndPoint, ILoggerFactory? loggerFactory) : base(iPEndPoint, loggerFactory) {
    }

    /// <inheritdoc />
    public override bool Connected => _wrapped is { Connected: true };

    /// <inheritdoc />
    public override void Close() {
        try {
            _wrapped?.Close();
            _wrapped?.Dispose();
        } finally {
            _wrapped = null;
        }
    }

    /// <inheritdoc />
    public override void Reset() {
        Close();
        _wrapped = new TcpClient();
    }

    /// <inheritdoc />
    public override IDeviceStream<string> GetStream() {
        if (_wrapped is not { Connected: true }) throw new DeviceClientException($"{_wrapped?.GetType().Name} is closed or null");
        return new HttpStream(new SocketWrapper(_wrapped.Client, true), LoggerFactory);
    }

    /// <inheritdoc />
    public override void Open() {
        if (_wrapped is { Connected: true }) return;
        Reset();
        _wrapped?.Connect(Params);
    }

    /// <param name="token"></param>
    /// <inheritdoc />
    public override Task OpenAsync(CancellationToken token) {
        if (_wrapped is { Connected: true }) return Task.CompletedTask;
        Reset();
        return _wrapped?.ConnectAsync(Params.Address, Params.Port)!;
    }

    /// <inheritdoc />
    public override void Dispose() => Close();
}