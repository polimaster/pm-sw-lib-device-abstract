using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

public class TcpClientAdapter : IClient<HttpConnectionParams> {
    private readonly TcpClient _wrapped;

    public TcpClientAdapter() {
        _wrapped = new TcpClient();
    }

    public bool Connected => _wrapped.Connected;

    public void Close() {
        _wrapped.Close();
    }

    public Task<IDeviceStream> GetStream() {
        return Task.FromResult<IDeviceStream>(new TcpStream(_wrapped.GetStream()));
    }

    public void Open(HttpConnectionParams connectionParams) {
        _wrapped.Connect(connectionParams.Ip, connectionParams.Port);
    }

    public async Task OpenAsync(HttpConnectionParams connectionParams) {
        await _wrapped.ConnectAsync(connectionParams.Ip, connectionParams.Port);
    }

    public void Dispose() {
        _wrapped.Dispose();
    }
}