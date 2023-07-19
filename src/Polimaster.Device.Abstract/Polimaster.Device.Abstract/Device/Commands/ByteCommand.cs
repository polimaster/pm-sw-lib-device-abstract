using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

public abstract class ByteCommand<T> : ACommand<T, byte[]> {
    
    /// <summary>
    /// Max data length while reading transport stream
    /// </summary>
    protected const int MAX_DATA_LENGTH = 10000;
    
    protected override async Task Write(CancellationToken cancellationToken = new()) {
        await Device.Semaphore.WaitAsync(cancellationToken);
        try {
            await WriteInternal(cancellationToken);
        } finally {
            Device.Semaphore.Release();
        }
    }

    private async Task WriteInternal(CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Call {N} with command {C}", nameof(Write), GetType().Name);
        Validate();
        var stream = await Device.Transport.Open();
        var command = Compile();
        Logger?.LogDebug("Writing {C} byte(s)", command.Length);
        await stream.WriteAsync(command, 0, command.Length, cancellationToken);
        ValueChanged?.Invoke(Value);
    }

    protected override async Task Read(CancellationToken cancellationToken = new()) {
        
        await Device.Semaphore.WaitAsync(cancellationToken);

        try {
            await WriteInternal(cancellationToken);
        
            Logger?.LogDebug("Call {N} with command {C}", nameof(Read), GetType().Name);
        
            var stream = await Device.Transport.Open();
        
            // var data = Array.Empty<byte>();
            var buff = new byte[MAX_DATA_LENGTH];
            var len = await stream.ReadAsync(buff, 0, MAX_DATA_LENGTH, cancellationToken);

            Logger?.LogDebug("Received from transport stream {C} byte(s)", len);

            // if (len <= 0) return data;
            var data = new byte[len];
            for (var i = 0; i < len; i++) data[i] = buff[i];

            Logger?.LogDebug("Got response: {A} byte(s)", data.Length);
        
            Value = Parse(data);
            ValueChanged?.Invoke(Value);
            
        } finally {
            Device.Semaphore.Release();
        }
    }
}