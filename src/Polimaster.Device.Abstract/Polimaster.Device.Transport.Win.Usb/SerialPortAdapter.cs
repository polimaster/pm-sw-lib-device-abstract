using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Transport.Win.Usb;

/// <inheritdoc />
public class SerialPortAdapter : AClient<string, UsbDevice> {
    private DevicePort? _wrapped;
    
    /// <summary>
    /// See <see cref="SerialPort.BaudRate"/>
    /// </summary>
    protected virtual int BaudRate => 9600;
    /// <summary>
    /// See <see cref="SerialPort.ReadTimeout"/>, <see cref="SerialPort.WriteTimeout"/>
    /// </summary>
    protected virtual int Timeout => 2000;
    /// <summary>
    /// See <see cref="SerialPort.ReadBufferSize"/>
    /// </summary>
    protected virtual int BufferSize => 4096;
    
    /// <summary>
    /// See <see cref="SerialPort.DataBits"/>
    /// </summary>
    protected virtual int DataBits => 8;
    
    /// <summary>
    /// See <see cref="SerialPort.NewLine"/>
    /// </summary>
    protected virtual string Newline => "\r\n";
    
    /// <summary>
    /// See <see cref="SerialPort.Encoding"/>
    /// </summary>
    protected virtual Encoding Encoding => Encoding.UTF8;

    /// <inheritdoc />
    public SerialPortAdapter(UsbDevice @params, ILoggerFactory? loggerFactory) : base(@params, loggerFactory) {
    }

    /// <inheritdoc />
    public override bool Connected => _wrapped is { IsOpen: true };

    /// <inheritdoc />
    public override void Close() {
        _wrapped?.Close();
        _wrapped?.Dispose();
        _wrapped = null;
    }

    /// <inheritdoc />
    public override void Reset() {
        Close();
        _wrapped = new DevicePort(Params.Name, BaudRate, Parity.None, DataBits, StopBits.One);
        _wrapped.ReadBufferSize = BufferSize;
        _wrapped.ReadTimeout = Timeout;
        _wrapped.WriteTimeout = Timeout;
        _wrapped.Encoding = Encoding;
        _wrapped.NewLine = Newline;
    }

    /// <inheritdoc />
    public override IDeviceStream<string> GetStream() {
        if (_wrapped is not { IsOpen: true }) throw new DeviceClientException($"{_wrapped?.GetType().Name} is closed or null");
        return new SerialPortStream(_wrapped, LoggerFactory);
    }

    /// <inheritdoc />
    public override void Open() {
        if (_wrapped is { IsOpen: true }) return;

        Reset();
        _wrapped?.Open();
    }

    /// <inheritdoc />
    public override async Task OpenAsync(CancellationToken token) {
        Open();
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void Dispose() => Close();
}