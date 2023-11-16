using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <inheritdoc cref="Polimaster.Device.Abstract.Transport.IClient{TConnectionParams}" />
public class TcpClientAdapter : IClient<HttpConnectionParams> {
    private readonly TcpClient _wrapped;
    
    /// <summary>
    /// 
    /// </summary>
    public TcpClientAdapter() {
        _wrapped = new TcpClient();
    }

    /// <inheritdoc />
    public bool Connected => _wrapped.Connected;

    /// <inheritdoc />
    public void Close() {
        _wrapped.Close();
    }

    /// <inheritdoc />
    public Task<IDeviceStream> GetStream() {
        return Task.FromResult<IDeviceStream>(new TcpStream(_wrapped.GetStream()));
    }

    /// <inheritdoc />
    public void Open(HttpConnectionParams connectionParams) {
        _wrapped.Connect(connectionParams.Ip, connectionParams.Port);
    }

    /// <inheritdoc />
    public async Task OpenAsync(HttpConnectionParams connectionParams) {
        await _wrapped.ConnectAsync(connectionParams.Ip, connectionParams.Port);
    }

    /// <inheritdoc />
    public void Dispose() {
        _wrapped.Dispose();
    }
}