# Setting Up Device Transport

## Define connection parameters

Connection parameters describe how to reach a physical device (e.g., COM port name and PnP ID for USB).

```c#
public class UsbConnectionParams : IFormattable {
    public string Name { get; set; } = null!;
    public string PlugAndPlayId { get; set; } = null!;

    public string ToString(string? format, IFormatProvider? formatProvider) => $"{Name}:{PlugAndPlayId}";
    public override string ToString() => $"{Name}:{PlugAndPlayId}";
}
```

## Define a client

A client wraps the physical connection. Extend `AClient<TStream, TConnectionParams>`:

```c#
public interface IUsbClient : IClient<Stream> {
}

public class UsbClient : AClient<Stream, UsbConnectionParams>, IUsbClient {
    private SerialPort? _port;
    private const int BAUD_RATE = 9600;
    private const int TIMEOUT = 2000;
    private const int BUFFER_SIZE = 4096;

    public UsbClient(UsbConnectionParams @params, ILoggerFactory? loggerFactory = null)
        : base(@params, loggerFactory) { }

    public override bool Connected => _port is { IsOpen: true };

    public override Task Open(CancellationToken token) {
        _port ??= new SerialPort(Params.Name, BAUD_RATE, Parity.None, 8, StopBits.One) {
            ReadBufferSize = BUFFER_SIZE,
            ReadTimeout = TIMEOUT,
            WriteTimeout = TIMEOUT,
            Encoding = Encoding.ASCII
        };
        if (!_port.IsOpen) _port.Open();
        return Task.CompletedTask;
    }

    public override void Close() {
        _port?.Close();
        _port?.Dispose();
        _port = null;
    }

    public override Stream GetStream() {
        if (_port == null) throw new InvalidOperationException("Port not open");
        return _port.BaseStream;
    }

    public override void Reset() {
        Close();
    }

    public override void Dispose() => Close();
}
```

Key points:
- `Params` is available in the base class (the `TConnectionParams` passed to the constructor).
- `ConnectionId` defaults to `"{ClassName}#{Params}"` — override if needed.
- `GetStream()` returns the raw stream; commands will use this stream directly.

## Define a device stream (optional wrapper)

If you need structured read/write helpers (e.g., line-based or binary framing), implement `IDeviceStream<T>`:

```c#
// For line-based (string) communication:
public class UsbStringStream : IDeviceStream<string> {
    private const string NEWLINE = "\r\n";
    private readonly SerialPort _port;

    public UsbStringStream(SerialPort port) => _port = port;

    public Task Write(string data, CancellationToken cancellationToken) {
        _port.Write(data + NEWLINE);
        return Task.CompletedTask;
    }

    public Task<string> Read(CancellationToken cancellationToken) =>
        Task.FromResult(_port.ReadTo(NEWLINE));
}
```

`IDeviceStream<T>` is a generic abstraction — use `string` for text-based protocols or `byte[]` for binary ones. Your commands will work directly with whatever `TStream` type the client's `GetStream()` returns, so a wrapper is only needed if you want to add framing/encoding logic.

## Define a transport

A transport wraps the client and manages thread-safe stream access. Extend `ATransport<TStream>`:

```c#
public interface IPm1703Transport : ITransport<Stream> {
}

public class Pm1703Transport : ATransport<Stream>, IPm1703Transport {
    public Pm1703Transport(IClient<Stream> client, ILoggerFactory? loggerFactory = null)
        : base(client, loggerFactory) { }

    // Override these if needed:
    // protected override bool SyncStreamAccess => true;  // default: true — use semaphore
    // protected override bool KeepOpen => false;          // default: false — close after each command
    // protected override ushort Sleep => 1;               // default: 1 ms pause between commands
}
```

`ATransport` provides:
- `ExecOnStream(action, token)` — opens the connection (if not already open), runs the action on the stream, and auto-retries once on transient failure.
- Thread-safe access via `SemaphoreSlim` when `SyncStreamAccess` is `true`.
- Automatic open/close lifecycle controlled by `KeepOpen`.
