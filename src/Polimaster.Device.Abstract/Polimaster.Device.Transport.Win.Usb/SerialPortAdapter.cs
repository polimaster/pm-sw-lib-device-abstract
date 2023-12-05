using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Transport.Win.Usb;

/// <inheritdoc />
public class SerialPortAdapter : AClient<string, UsbDevice> {
    private SerialPort? _port;
    private const int BAUD_RATE = 9600;
    private const int TIMEOUT = 2000;
    private const int BUFFER_SIZE = 4096;
    private const int DATA_BITS = 8;

    /// <inheritdoc />
    public SerialPortAdapter(UsbDevice @params, ILoggerFactory? loggerFactory) : base(@params, loggerFactory) {
        Reset();
    }

    /// <inheritdoc />
    public override bool Connected => _port is { IsOpen: true };

    /// <inheritdoc />
    public override void Close() {
        _port?.Close();
        _port?.Dispose();
        _port = null;
    }

    /// <inheritdoc />
    public sealed override void Reset() {
        Close();
        _port = new SerialPort(Params.Name, BAUD_RATE, Parity.None, DATA_BITS, StopBits.One);
        _port.ReadBufferSize = BUFFER_SIZE;
        _port.ReadTimeout = TIMEOUT;
        _port.WriteTimeout = TIMEOUT;
        _port.Encoding = Encoding.UTF8;
    }

    /// <inheritdoc />
    public override IDeviceStream<string> GetStream() {
        if (_port is not { IsOpen: true }) throw new DeviceClientException($"{_port?.GetType().Name} is closed or null");
        return new SerialPortStream(_port, LoggerFactory);
    }

    /// <inheritdoc />
    public override void Open() {
        if (_port is { IsOpen: true }) return;

        Reset();
        _port?.Open();
    }

    /// <inheritdoc />
    public override async Task OpenAsync(CancellationToken token) {
        Open();
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Dispose() => Close();
}