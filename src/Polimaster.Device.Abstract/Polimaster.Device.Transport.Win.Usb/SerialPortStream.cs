using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Transport.Win.Usb;

/// <inheritdoc />
public class SerialPortStream : IDeviceStream<string> {
    private readonly SerialPort _serialPort;
    private readonly ILogger<SerialPortStream>? _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serialPort"></param>
    /// <param name="loggerFactory"></param>
    public SerialPortStream(SerialPort serialPort, ILoggerFactory? loggerFactory = null) {
        _serialPort = serialPort;
        _logger = loggerFactory?.CreateLogger<SerialPortStream>();
    }

    /// <inheritdoc />
    public virtual async Task WriteAsync(string buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteAsync), buffer);
        _serialPort.WriteLine(buffer);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public virtual Task<string> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        var res = _serialPort.ReadTo(_serialPort.NewLine);
        return Task.FromResult(res);
    }
}