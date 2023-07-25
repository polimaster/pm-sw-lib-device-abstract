using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

public abstract class ByteCommand<T> : ACommand<T, byte[]> {
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

    private async Task WriteInternal(CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Call {N} with command {C}", nameof(Write), GetType().Name);
        Validate();
        var command = Compile();
        var stream = await Device.Transport.Open();
        await stream.WriteAsync(command, cancellationToken);
        Thread.Sleep(10);
    }

    protected override async Task Read(CancellationToken cancellationToken = new()) {
        await Device.Semaphore.WaitAsync(cancellationToken);
        try {
            await WriteInternal(cancellationToken);

            Logger?.LogDebug("Call {N} with command {C}", nameof(Read), GetType().Name);
            
            var stream = await Device.Transport.Open();
            var data = await stream.ReadAsync(cancellationToken);
            
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