using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Http;

/// <inheritdoc />
public class HttpAdapter : AClient<string, EndPoint> {
    private TcpClient? _wrapped;

    /// <inheritdoc />
    public HttpAdapter(EndPoint iPEndPoint, ILoggerFactory? loggerFactory) : base(iPEndPoint, loggerFactory) {
    }

    /// <inheritdoc />
    public override bool Connected => _wrapped is { Connected: true };

    /// <inheritdoc />
    public override void Close() {
        _wrapped?.Close();
        _wrapped?.Dispose();
        _wrapped = null;
    }

    /// <inheritdoc />
    public override void Reset() {
        Close();
        _wrapped = new TcpClient();
    }

    /// <inheritdoc />
    public override IDeviceStream<string> GetStream() {
        if (_wrapped is not { Connected: true }) throw new DeviceClientException($"{_wrapped?.GetType().Name} is closed or null");
        return new HttpStream(_wrapped, LoggerFactory);
    }

    /// <inheritdoc />
    public override void Open() {
        if (_wrapped is { Connected: true }) return;
        Reset();
        _wrapped?.Connect(Params);
    }

    /// <param name="token"></param>
    /// <inheritdoc />
    public override async Task OpenAsync(CancellationToken token) {
        if (_wrapped is { Connected: true }) return;
        Reset();
        await _wrapped?.ConnectAsync(Params.Address, Params.Port)!;
    }

    /// <inheritdoc />
    public override void Dispose() => Close();
}