using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command for sending string to data stream
/// </summary>
/// <typeparam name="T">Type of command value</typeparam>
public abstract class StringCommandWrite<T> : WriteCommand<T, string> {
    /// <inheritdoc />
    protected StringCommandWrite(ILoggerFactory? loggerFactory = null) : base(loggerFactory) {
    }

    /// <inheritdoc />
    protected override async Task WriteData<TStream>(TStream stream, string command, CancellationToken cancellationToken) => 
        await stream.WriteAsync(command, cancellationToken);
}