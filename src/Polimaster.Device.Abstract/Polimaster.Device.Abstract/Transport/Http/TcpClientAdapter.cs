using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

public class TcpClientAdapter : AClient<HttpConnectionParams>, IClient<HttpConnectionParams> {
    
    public override Action? Opened { get; set; }
    public override Action? Closed { get; set; }

    private readonly TcpClient _wrapped;

    public TcpClientAdapter() {
        _wrapped = new TcpClient();
    }

    public override bool Connected => _wrapped.Connected;

    public override void Close() {
        _wrapped.Close();
    }

    public override Task<IDeviceStream> GetStream() {
        return Task.FromResult<IDeviceStream>(new TcpStream(_wrapped.GetStream()));
    }

    public override void Open(HttpConnectionParams connectionParams) {
        _wrapped.Connect(connectionParams.Ip, connectionParams.Port);
    }

    public override async Task OpenAsync(HttpConnectionParams connectionParams) {
        await _wrapped.ConnectAsync(connectionParams.Ip, connectionParams.Port);
    }

    
    public override void Dispose() {
        _wrapped.Dispose();
    }
}