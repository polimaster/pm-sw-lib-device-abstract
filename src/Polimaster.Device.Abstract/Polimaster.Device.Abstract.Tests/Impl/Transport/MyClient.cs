using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Tests.Impl.Transport.Socket;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Tests.Impl.Transport;

public class MyClient(ClientParams @params, ILoggerFactory? loggerFactory)
    : AClient<IMyDeviceStream, ClientParams>(@params, loggerFactory) {
    private TcpClient? _wrapped;

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
    public override IMyDeviceStream GetStream() {
        if (_wrapped is not { Connected: true }) throw new DeviceClientException($"{_wrapped?.GetType().Name} is closed or null");
        return new MyDeviceStream(new SocketWrapper(_wrapped.Client, true), LoggerFactory);
    }


    /// <param name="token"></param>
    /// <inheritdoc />
    public override async Task Open(CancellationToken token) {
        if (_wrapped is { Connected: true }) return;
        Reset();
        if (_wrapped != null) {
            await _wrapped.ConnectAsync(Params.Address, Params.Port, token);
        }
    }

    /// <inheritdoc />
    public override void Dispose() => Close();
}