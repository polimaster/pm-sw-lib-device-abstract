using System;
using System.Data;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport; 

public abstract class Http<TData> : ITransport<TData> {
    private readonly string _ip;
    private readonly int _port;
    private readonly int _connectTimeout;
    private readonly int _readWriteTimeout;
    private TcpClient? _client;

    /// <inheritdoc cref="ITransport{TData}.ConnectionState"/>
    public virtual ConnectionState ConnectionState {
        get {
            if (_client == null) return ConnectionState.Closed;
            lock (_client) { return _client.Connected ? ConnectionState.Open : ConnectionState.Closed; }
        }
    }

    /// <inheritdoc cref="ITransport{TData}.ConnectionStateChanged"/>
    public virtual event Action<ConnectionState>? ConnectionStateChanged;

    protected Http(string ip, int port, int connectTimeout = 5000, int readWriteTimeout = 5000) {
        _ip = ip;
        _port = port;
        _connectTimeout = connectTimeout;
        _readWriteTimeout = readWriteTimeout;
        InitClient();
    }

    /// <summary>
    /// Initialize TcpClient
    /// </summary>
    protected void InitClient() {
        lock (_client!) {
            _client?.Close();
            _client?.Dispose();
            _client = new TcpClient();
            _client.ReceiveTimeout = _readWriteTimeout;
            _client.SendTimeout = _readWriteTimeout;
        }
    }

    /// <inheritdoc cref="ITransport{TData}.Open"/>
    public virtual Task Open() {
        lock (_client!) {
            if (ConnectionState == ConnectionState.Open) return Task.CompletedTask;
            var connected = _client.ConnectAsync(_ip, _port).Wait(_connectTimeout);
            if (!connected) throw new TimeoutException($"Connection to {_ip}:{_port} timed out");
            ConnectionStateChanged?.Invoke(ConnectionState);
            return Task.CompletedTask;
        }
    }

    /// <inheritdoc cref="ITransport{TData}.Close"/>
    public virtual Task Close() {
        lock (_client!) {
            InitClient();
            ConnectionStateChanged?.Invoke(ConnectionState);
            return Task.CompletedTask;
        }
    }
    
    
    /// <inheritdoc cref="ITransport{TData}.Write"/>
    public abstract Task Write(TData command);

    /// <inheritdoc cref="ITransport{TData}.Read"/>
    public abstract Task<TData> Read(TData command);

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() {
        _client?.Close();
        _client?.Dispose();
    }
}