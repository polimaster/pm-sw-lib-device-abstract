using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Transport.Win.Usb;


/// <inheritdoc />
public class SerialPortStream : IDeviceStream<string> {
    private readonly IDevicePort _port;
    private readonly ILogger<SerialPortStream>? _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="port"></param>
    /// <param name="loggerFactory"></param>
    public SerialPortStream(IDevicePort port, ILoggerFactory? loggerFactory = null) {
        _port = port;
        _logger = loggerFactory?.CreateLogger<SerialPortStream>();
    }

    /// <inheritdoc />
    public virtual Task WriteAsync(string buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteAsync), buffer);
        _port.WriteLine(buffer);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task<string> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        var res = _port.ReadTo(_port.NewLine);
        return Task.FromResult(res);
    }
}