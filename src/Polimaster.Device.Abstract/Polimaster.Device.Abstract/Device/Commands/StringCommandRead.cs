using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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
    protected override async Task<string> ReadData<TStream>(TStream stream, CancellationToken cancellationToken) => 
        await stream.ReadAsync(cancellationToken);

    /// <inheritdoc />
    protected override async Task WriteData<TStream>(TStream stream, string command, CancellationToken cancellationToken) => 
        await stream.WriteAsync(command, cancellationToken);
}