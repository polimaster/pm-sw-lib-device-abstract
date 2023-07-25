using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

public abstract class StringCommand<T> : ACommand<T, string> {

    protected override async Task Write(CancellationToken cancellationToken = new()) {
        await Device.Semaphore.WaitAsync(cancellationToken);
        try {
            await WriteInternal(cancellationToken);
            ValueChanged?.Invoke(Value);
        } catch (Exception e) {
            Logger?.LogError(e, "Error while sending {N} command {C}",nameof(Write), GetType().Name);
            await Device.Transport.Close();
        } finally {
            Device.Semaphore.Release();
        }
    }
    
    private async Task WriteInternal(CancellationToken cancellationToken) {
        Logger?.LogDebug("Call {N} with command {C}", nameof(Write), GetType().Name);
        Validate();
        var stream = await Device.Transport.Open();
        var command = Compile();
        Logger?.LogDebug("Writing {C}", command);
        await stream.WriteLineAsync(command, cancellationToken);
        Thread.Sleep(1);
    }

    protected override async Task Read(CancellationToken cancellationToken = new()) {
        
        await Device.Semaphore.WaitAsync(cancellationToken);
        
        try {
            await WriteInternal(cancellationToken);

            Logger?.LogDebug("Call {N} with command {C}", nameof(Read), GetType().Name);
        
            var reader = await Device.Transport.Open();
            var data = await reader.ReadLineAsync(cancellationToken);
            Thread.Sleep(1);

            Value = Parse(data);
            ValueChanged?.Invoke(Value);
            
        } catch (Exception e) {
            Logger?.LogError(e, "Error while sending {N} command {C}",nameof(Read), GetType().Name);
            await Device.Transport.Close();
        } finally {
            Device.Semaphore.Release();
        }
        
    }
    
}