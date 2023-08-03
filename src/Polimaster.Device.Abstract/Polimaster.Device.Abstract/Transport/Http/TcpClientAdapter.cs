using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

/// <inheritdoc cref="Polimaster.Device.Abstract.Transport.IClient{TConnectionParams}" />
public class TcpClientAdapter : AClient<HttpConnectionParams>, IClient<HttpConnectionParams> {
    /// <inheritdoc />
    public override Action? Opened { get; set; }

    /// <inheritdoc />
    public override Action? Closed { get; set; }

    private readonly TcpClient _wrapped;

    /// <inheritdoc />
    public TcpClientAdapter() {
        _wrapped = new TcpClient();
    }

    /// <inheritdoc />
    public override bool Connected => _wrapped.Connected;

    /// <inheritdoc />
    public override void Close() {
        _wrapped.Close();
    }

    /// <inheritdoc />
    public override Task<IDeviceStream> GetStream() {
        return Task.FromResult<IDeviceStream>(new TcpStream(_wrapped.GetStream()));
    }

    /// <inheritdoc />
    public override void Open(HttpConnectionParams connectionParams) {
        _wrapped.Connect(connectionParams.Ip, connectionParams.Port);
    }

    /// <inheritdoc />
    public override async Task OpenAsync(HttpConnectionParams connectionParams) {
        await _wrapped.ConnectAsync(connectionParams.Ip, connectionParams.Port);
    }


    /// <inheritdoc />
    public override void Dispose() {
        _wrapped.Dispose();
    }
}