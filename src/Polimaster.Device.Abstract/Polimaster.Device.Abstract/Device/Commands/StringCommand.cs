using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

public abstract class StringCommand<T> : ACommand<T, string> {

    protected override async Task Write(CancellationToken cancellationToken = new()) {
        Validate();
        var writer = Device.Transport.GetWriter();
        var command = Compile();
        Logger?.LogDebug("Writing command: {C}", command);
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);
        ValueChanged?.Invoke(Value);
    }

    protected override async Task Read(CancellationToken cancellationToken = new()) {
        await Write(cancellationToken);
        Logger?.LogDebug("Reading command response data");
        
        var reader = Device.Transport.GetReader();
        var response = await reader.ReadToEndAsync();
        Logger?.LogDebug("Got response: {A} ", response);
        
        Value = Parse(response);
        ValueChanged?.Invoke(Value);
    }
}