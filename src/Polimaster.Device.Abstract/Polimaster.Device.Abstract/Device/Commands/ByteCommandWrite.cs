using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command for sending byte[] to data stream
/// </summary>
/// <typeparam name="T">Type of command value</typeparam>
public abstract class ByteCommandWrite<T> : WriteCommand<T, byte[]> {
    /// <inheritdoc />
    protected ByteCommandWrite(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    protected override async Task WriteData<TStream>(TStream stream, byte[] command, CancellationToken cancellationToken) => 
        await stream.WriteAsync(command, cancellationToken);
}