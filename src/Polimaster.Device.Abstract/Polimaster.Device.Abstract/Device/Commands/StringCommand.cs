using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

public abstract class StringCommand<T> : ACommand<T, string> {

    protected override async Task Write(CancellationToken cancellationToken = new()) {
        await Device.Semaphore.WaitAsync(cancellationToken);
        try {
            await WriteInternal(cancellationToken);
        } finally {
            Device.Semaphore.Release();
        }
    }
    
    private async Task WriteInternal(CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Call {N} command {C}", nameof(Write), GetType().Name);
        Validate();
        var writer = await Device.Transport.GetWriter();
        var command = Compile();
        Logger?.LogDebug("Writing {C}", command);
        await writer.WriteLineAsync(command.ToCharArray(), cancellationToken);
        ValueChanged?.Invoke(Value);
    }

    protected override async Task Read(CancellationToken cancellationToken = new()) {
        
        await Device.Semaphore.WaitAsync(cancellationToken);
        
        try {
            await WriteInternal(cancellationToken);

            Logger?.LogDebug("Call {N} command {C}", nameof(Read), GetType().Name);
        
            var reader = await Device.Transport.GetReader();
            var response = await reader.ReadLineAsync();
            Logger?.LogDebug("Got response: {A} ", response);
        
            Value = Parse(response);
            ValueChanged?.Invoke(Value);
            
        } finally {
            Device.Semaphore.Release();
        }
        
    }
    
}