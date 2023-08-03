using System;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Commands;


/// <summary>
/// For sending string commands to device
/// </summary>
/// <typeparam name="T">Type of command value</typeparam>
public abstract class StringCommand<T> : ACommand<T, string> {
    
    /// <inheritdoc />
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
    
    private async Task WriteInternal(CancellationToken cancellationToken) {
        LogCommand(nameof(Write));
        Validate();
        var stream = await Device.Transport.Open();
        var command = Compile();
        await stream.WriteLineAsync(command, cancellationToken);
        Thread.Sleep(1);
    }

    /// <inheritdoc />
    protected override async Task Read(CancellationToken cancellationToken = new()) {
        
        await Device.Semaphore.WaitAsync(cancellationToken);
        
        try {
            await WriteInternal(cancellationToken);
            
            LogCommand(nameof(Read));

            var reader = await Device.Transport.Open();
            var data = await reader.ReadLineAsync(cancellationToken);
            Thread.Sleep(1);

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