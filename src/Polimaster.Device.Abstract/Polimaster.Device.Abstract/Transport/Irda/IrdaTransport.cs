using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Irda;

public class IrdaTransport<TConnectionParams> : ATransport<byte[], TConnectionParams>, IIrdaTransport<TConnectionParams> {

    /// <summary>
    /// Max data length while reading transport stream
    /// </summary>
    private const int MAX_DATA_LENGTH = 10000;

    /// <summary>
    /// </summary>
    /// <param name="client"></param>
    /// <param name="connectionParams">Parameters for underlying client, <see cref="IIrdaTransport{TConnectionParams}" /></param>
    /// <param name="loggerFactory"></param>
    public IrdaTransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }

    public override async Task Write(Stream stream, byte[] command, CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing to transport stream {C} byte(s)", command.Length);
        await stream.WriteAsync(command, 0, command.Length, cancellationToken);
    }

    public override async Task<byte[]> Read(Stream stream, byte[] command, CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing to transport stream {C} byte(s)", command.Length);
        await stream.WriteAsync(command, 0, command.Length, cancellationToken);

        Logger?.LogDebug("Reading from transport stream");
        var data = Array.Empty<byte>();
        var buff = new byte[MAX_DATA_LENGTH];
        var len = await stream.ReadAsync(buff, 0, MAX_DATA_LENGTH, cancellationToken);

        Logger?.LogDebug("Received from transport stream {C} byte(s)", len);

        if (len <= 0) return data;
        data = new byte[len];
        for (var i = 0; i < len; i++) data[i] = buff[i];

        return data;
    }

    public override Task<Stream?> Open() {
        if (Client.Connected) return Task.FromResult<Stream?>(Client.GetStream());
        Logger?.LogDebug("Opening transport connection to device {A}", ConnectionParams);
        Client.Connect(ConnectionParams);
        return Task.FromResult<Stream?>(Client.GetStream());
    }

    public override Task Close() {
        Logger?.LogDebug("Closing transport connection to device {A}", ConnectionParams);
        Client.Close();
        return Task.CompletedTask;
    }

    public override void Dispose() {
        Close();
    }
}