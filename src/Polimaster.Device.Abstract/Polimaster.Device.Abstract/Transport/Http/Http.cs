using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Http;

public class Http<TTcpClient> : ITransport<string, HttpConnectionParams> where TTcpClient : ITcpClient {
    private readonly TTcpClient _client;

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.ConnectionState"/>
    public virtual ConnectionState ConnectionState => _client.Connected ? ConnectionState.Open : ConnectionState.Closed;

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.ConnectionStateChanged"/>
    public virtual event Action<ConnectionState>? ConnectionStateChanged;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client">Transport client</param>
    public Http(TTcpClient client) {
        _client = client;
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Open"/>
    public virtual async Task<Stream> Open(HttpConnectionParams connectionParams) {
        if (ConnectionState == ConnectionState.Open) _client.GetStream();

        var connected = _client.ConnectAsync(connectionParams.Ip, connectionParams.Port).Wait(connectionParams.Timeout);
        if (!connected) throw new TimeoutException($"Connection to {connectionParams.Ip}:{connectionParams.Port} timed out");
        ConnectionStateChanged?.Invoke(ConnectionState);
        return _client.GetStream();
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Close"/>
    public virtual async Task Close() {
        _client.Close();
        ConnectionStateChanged?.Invoke(ConnectionState);
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Write"/>
    public virtual async Task Write(Stream stream, string command, CancellationToken cancellationToken) {
        await using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);
        writer.Close();
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Read"/>
    public virtual async Task<string> Read(Stream stream, string command, CancellationToken cancellationToken) {
        await using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);
        // writer.Close();

        using var reader = new StreamReader(stream);
        var response = await reader.ReadToEndAsync();
        // reader.Close();
        return response;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose() {
        _client.Dispose();
    }
}