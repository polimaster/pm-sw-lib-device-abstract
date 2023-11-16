using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command for reading data while sending string to data stream
/// </summary>
/// <typeparam name="T">Type of command value</typeparam>
public abstract class StringCommandRead<T> : ReadCommand<T, string> {
    /// <inheritdoc />
    protected StringCommandRead(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    protected override async Task WriteData(IDeviceStream stream, string command, CancellationToken cancellationToken) =>
        await stream.WriteLineAsync(command, cancellationToken);

    /// <inheritdoc />
    protected override async Task<string> ReadData(IDeviceStream stream, CancellationToken cancellationToken) => 
        await stream.ReadLineAsync(cancellationToken);
}