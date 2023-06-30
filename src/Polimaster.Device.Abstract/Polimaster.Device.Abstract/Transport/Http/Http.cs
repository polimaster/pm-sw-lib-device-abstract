using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Transport.Http;

public class Http : ITransport<string, HttpConnectionParams> {
    public IClient<HttpConnectionParams> Client { get; }
    public HttpConnectionParams ConnectionParams { get; }
    
    public string ConnectionId => $"{GetType().Name}:{ConnectionParams.Ip}:{ConnectionParams.Port}";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client">Transport client</param>
    /// <param name="connectionParams"></param>
    public Http(IClient<HttpConnectionParams> client, HttpConnectionParams connectionParams) {
        ConnectionParams = connectionParams;
        Client = client;
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Open"/>
    public Task<Stream?> Open() {
        if (Client.Connected) Client.GetStream();

        var connected = Client.ConnectAsync(ConnectionParams).Wait(ConnectionParams.Timeout);
        if (!connected) throw new TimeoutException($"Connection to {ConnectionParams.Ip}:{ConnectionParams.Port} timed out");
        return Task.FromResult<Stream?>(Client.GetStream());
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Close"/>
    public Task Close() {
        Client.Close();
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Write"/>
    public async Task Write(Stream stream, string command, CancellationToken cancellationToken) {
        await using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);
        writer.Close();
    }

    /// <inheritdoc cref="ITransport{TData, HttpConnectionParams}.Read"/>
    public async Task<string> Read(Stream stream, string command, CancellationToken cancellationToken) {
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
        Client.Dispose();
    }
}