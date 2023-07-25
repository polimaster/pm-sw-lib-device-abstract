using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Commands;

public abstract class ByteCommand<T> : ACommand<T, byte[]> {
    protected override async Task Write(CancellationToken cancellationToken = new()) {
        await Device.Semaphore.WaitAsync(cancellationToken);
        try {
            await WriteInternal(cancellationToken);
            ValueChanged?.Invoke(Value);
        } catch (Exception e) {
            LogError(e, nameof(Write));
            await Device.Transport.Close();
        } finally {
            Device.Semaphore.Release();
        }
    }

    private async Task WriteInternal(CancellationToken cancellationToken = new()) {
        LogCommand(nameof(Write));
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

            LogCommand(nameof(Read));
            
            var stream = await Device.Transport.Open();
            var data = await stream.ReadAsync(cancellationToken);
            
            Value = Parse(data);
            ValueChanged?.Invoke(Value);
            
        } catch (Exception e) {
            LogError(e, nameof(Read));
            await Device.Transport.Close();
        } finally {
            Device.Semaphore.Release();
        }
    }
}