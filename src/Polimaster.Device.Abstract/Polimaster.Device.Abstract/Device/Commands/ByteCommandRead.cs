using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc />
public abstract class ByteCommandRead<T> : ReadCommand<T, byte[]> {
    /// <inheritdoc />
    protected ByteCommandRead(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    protected override async Task<byte[]> ReadData(IDeviceStream stream, CancellationToken cancellationToken) => 
        await stream.ReadAsync(cancellationToken);
}