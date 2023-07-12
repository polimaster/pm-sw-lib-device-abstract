using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport.Usb; 

public class UsbTransport<TConnectionParams> : ATransport<string, TConnectionParams>, IUsbTransport<TConnectionParams> {

    public UsbTransport(IClient<TConnectionParams> client, TConnectionParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }

    public override async Task Write(Stream stream, string command, CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing to transport stream {C} byte(s)", command.Length);
        await using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);
        writer.Close();
    }

    public override async Task<string> Read(Stream stream, string command, CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing to transport stream {C} byte(s)", command.Length);
        
        await using var writer = new StreamWriter(stream) { AutoFlush = true };
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);

        using var reader = new StreamReader(stream);
        var response = await reader.ReadToEndAsync();
        reader.Close();
        return response;
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