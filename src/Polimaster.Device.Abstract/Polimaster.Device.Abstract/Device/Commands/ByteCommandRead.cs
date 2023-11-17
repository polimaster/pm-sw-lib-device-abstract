using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc />
public abstract class ByteCommandRead<T> : ReadCommand<T, byte[]> {
    /// <inheritdoc />
    protected ByteCommandRead(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    protected override async Task<byte[]> ReadData<TStream>(TStream stream, CancellationToken cancellationToken) => 
        await stream.ReadAsync(cancellationToken);

    /// <inheritdoc />
    protected override async Task WriteData<TStream>(TStream stream, byte[] command, CancellationToken cancellationToken) => 
        await stream.WriteAsync(command, cancellationToken);
}