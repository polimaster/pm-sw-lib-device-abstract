using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

public abstract class Http : ITransport<string> {
    private readonly TcpClient _client;
    private readonly string _ip;
    private readonly int _port;
    private readonly int _connectTimeout;

    /// <inheritdoc cref="ITransport{TData}.ConnectionState"/>
    public ConnectionState ConnectionState {
        get {
            lock (_client) { return _client.Connected ? ConnectionState.Open : ConnectionState.Closed; }
        }
    }

    /// <inheritdoc cref="ITransport{TData}.ConnectionStateChanged"/>
    public event Action<ConnectionState>? ConnectionStateChanged;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client">Transport client</param>
    /// <param name="ip">IP address to connect</param>
    /// <param name="port">IP port to connect</param>
    /// <param name="connectTimeout">Connect timeout</param>
    protected Http(TcpClient client, string ip, int port, int connectTimeout = 5000) {
        _client = client;
        _ip = ip;
        _port = port;
        _connectTimeout = connectTimeout;
    }

    /// <inheritdoc cref="ITransport{TData}.Open"/>
    public virtual Task Open() {
        if (ConnectionState == ConnectionState.Open) return Task.CompletedTask;
        var connected = _client.ConnectAsync(_ip, _port).Wait(_connectTimeout);
        if (!connected) throw new TimeoutException($"Connection to {_ip}:{_port} timed out");
        ConnectionStateChanged?.Invoke(ConnectionState);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="ITransport{TData}.Close"/>
    public virtual Task Close() {
        _client.Close();
        ConnectionStateChanged?.Invoke(ConnectionState);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="ITransport{TData}.Write"/>
    public virtual Task Write(string command) {
        using var stream = _client.GetStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true };
        writer.WriteLine(command);
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="ITransport{TData}.Read"/>
    public virtual async Task<string> Read(string command) {
        await using var stream = _client.GetStream();

        await using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(command);

        using var reader = new StreamReader(stream);
        var response = await reader.ReadToEndAsync();
        return response;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() {
        _client.Close();
        _client.Dispose();
    }
}