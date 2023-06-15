using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

public class TcpClientAdapter : ITcpClient {
    private readonly TcpClient _wrapped;

    public TcpClientAdapter(TcpClient client) {
        _wrapped = client;
    }

    public bool Connected => _wrapped.Connected;

    public void Close() {
        _wrapped.Close();
    }

    public Stream GetStream() {
        return _wrapped.GetStream();
    }

    public async Task ConnectAsync(string ip, int port) {
        await _wrapped.ConnectAsync(ip, port);
    }

    public void Dispose() {
        _wrapped.Dispose();
    }
}