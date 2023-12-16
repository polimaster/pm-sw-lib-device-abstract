### Define connection parameters

```c#
public class UsbDeviceParams {
    public string Name { get; set; } = null!;
    public string PlugAndPlayId { get; set; } = null!;

    public override string ToString() {
        return $"{Name}:{PlugAndPlayId}";
    }
}
```

### Define Transport interface and it's implementation


```c#
public interface IPm1703Transport : ITransport<UsbDeviceParams> {
}

public class Pm1703Transport : ATransport<UsbDeviceParams>, IPm1703Transport {

    public Pm1703Transport(IClient<UsbDeviceParams> client, UsbDeviceParams connectionParams,
        ILoggerFactory? loggerFactory = null) : base(client, connectionParams, loggerFactory) {
    }
}
```

### Define USB client and Device stream

```c#
public interface IUsbClient : IClient<UsbDeviceParams>{
}

public class UsbClient : AClient<UsbDeviceParams>, IUsbClient {

    private SerialPort? _port;
    private UsbDeviceParams? _params;
    private ILogger<UsbClient>? _logger;
    private const int BAUD_RATE = 9600;
    private const int TIMEOUT = 2000;
    private const int BUFFER_SIZE = 4096;
    private const int DATA_BITS = 8;
    
    public override Action? Opened { get; set; }
    public override Action? Closed { get; set; }

    public override bool Connected => _port is { IsOpen: true };

    public override void Close() {
        _logger?.LogDebug("Closing transport connection to device {A}", _params);
        _port?.Close();
        _port?.Dispose();
        _port = null;
        Closed?.Invoke();
    }

    public override Task<IDeviceStream> GetStream() {
        if (_port == null) throw new NullReferenceException();
        return Task.FromResult<IDeviceStream>(new UsbStream(_port, LoggerFactory));
    }

    public override void Open(UsbDeviceParams connectionParams) {
        _params = connectionParams;
        _port ??= new SerialPort(connectionParams.Name, BAUD_RATE, Parity.None, DATA_BITS, StopBits.One);
        if (_port.IsOpen) return;
        
        _logger = LoggerFactory?.CreateLogger<UsbClient>();
        _logger?.LogDebug("Open transport connection to device {A}", _params);
        
        _port.ReadBufferSize = BUFFER_SIZE;
        _port.ReadTimeout = TIMEOUT;
        _port.WriteTimeout = TIMEOUT;
        _port.Encoding = Encoding.ASCII;
        _port.Open();
        Opened?.Invoke();
    }

    public override Task OpenAsync(UsbDeviceParams connectionParams) {
        Open(connectionParams);
        return Task.CompletedTask;
    }

    public override void Dispose() {
        Close();
    }
}

public class UsbStream : IDeviceStream {
    private const string NEWLINE = "\r\n";
    private readonly SerialPort _serialPort;
    private readonly ILogger<UsbStream>? _logger;
    public Stream BaseStream => _serialPort.BaseStream;


    public UsbStream(SerialPort serialPort, ILoggerFactory? loggerFactory = null) {
        _serialPort = serialPort;
        _logger = loggerFactory?.CreateLogger<UsbStream>();
    }

    public Task WriteLineAsync(string value, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V}", nameof(WriteLineAsync), value);
        _serialPort.Write(value + NEWLINE);
        return Task.CompletedTask;
    }

    public Task<string> ReadLineAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadLineAsync));
        return Task.FromResult(_serialPort.ReadTo(NEWLINE));
    }

    public Task WriteAsync(byte[] buffer, CancellationToken cancellationToken) {
        _logger?.LogDebug("Call {F} with: {V} bytes", nameof(WriteAsync), buffer.Length);
        _serialPort.Write(Encoding.ASCII.GetString(buffer));
        return Task.CompletedTask;
    }

    public Task<byte[]> ReadAsync(CancellationToken cancellationToken) {
        _logger?.LogDebug("Call: {F}", nameof(ReadAsync));
        var value = _serialPort.ReadTo(NEWLINE);
        // var value = _serialPort.ReadLine();
        var v = Encoding.ASCII.GetBytes(value);
        return Task.FromResult(v);
    }
}

```