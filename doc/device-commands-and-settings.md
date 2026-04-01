# Device Commands and Settings

## Define device commands

Commands encapsulate individual device operations. Use `AReader` for reading values and `AWriter` for writing.

### Reader command

Extend `AReader<TValue, TData, TStream>`:
- `TValue` — the typed value your command produces
- `TData` — the raw data type from the stream (e.g., `string` for text protocols, `byte[]` for binary)
- `TStream` — the stream type (e.g., `Stream`, `UsbStringStream`)

```c#
public class HistoryIntervalRead : AReader<ushort?, string, Stream> {
    public HistoryIntervalRead(ITransport<Stream> transport, ILoggerFactory? loggerFactory = null)
        : base(transport, loggerFactory) { }

    protected override async Task<string> Execute(Stream stream, CancellationToken cancellationToken) {
        // Write the query and read the response
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync("HInterval?");
        await writer.FlushAsync();
        var reader = new StreamReader(stream);
        return await reader.ReadLineAsync() ?? string.Empty;
    }

    protected override ushort? Parse(string? res) {
        if (res == null) throw new CommandResultParsingException("Null response");
        return ushort.Parse(res);
    }
}
```

### Writer command

Extend `AWriter<TValue, TData, TStream>`:

```c#
public class HistoryIntervalWrite : AWriter<ushort?, string, Stream> {
    public HistoryIntervalWrite(ITransport<Stream> transport, ILoggerFactory? loggerFactory = null)
        : base(transport, loggerFactory) { }

    protected override string Compile(ushort? value) =>
        $"HInterval={value}";

    protected override async Task Execute(Stream stream, string compiled, CancellationToken cancellationToken) {
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync(compiled);
        await writer.FlushAsync();
    }
}
```

### Command without a return value

For fire-and-forget operations (e.g., reset, sync time), extend `ACommand<TStream>`:

```c#
public class ResetDoseCommand : ACommand<Stream> {
    public ResetDoseCommand(ITransport<Stream> transport, ILoggerFactory? loggerFactory = null)
        : base(transport, loggerFactory) { }

    protected override async Task Execute(Stream stream, CancellationToken cancellationToken) {
        var writer = new StreamWriter(stream);
        await writer.WriteLineAsync("ResetDose");
        await writer.FlushAsync();
    }
}
```

Call it with: `await command.Exec(cancellationToken);`

### Exception types

Throw these from commands for predictable error handling:
- `CommandCompilationException` — thrown from `Compile()` when input is invalid
- `CommandResultParsingException` — thrown from `Parse()` when the device response is unexpected


## Define device settings

A setting wraps a reader and optional writer command. Build one using `SettingDefinition<T>`:

```c#
var historyInterval = new ADeviceSetting<ushort?>(new SettingDefinition<ushort?> {
    Descriptor = new SettingDescriptor(
        name: "History Interval",
        valueType: typeof(ushort?),
        groupName: "Logging",
        unit: "seconds"
    ),
    Reader = new HistoryIntervalRead(transport, loggerFactory),
    Writer = new HistoryIntervalWrite(transport, loggerFactory)
});
```

Settings are typically created inside the device's constructor or a dedicated initialization method. See [Device implementation](./device-implementation.md).

### Setting lifecycle

| Method | Behaviour |
|---|---|
| `Read(token)` | Reads from device; skips if already synchronized |
| `Reset(token)` | Forces re-read even if already synchronized |
| `CommitChanges(token)` | Writes to device only if `IsDirty && IsValid && !ReadOnly` |

### Key state flags

| Property | Description |
|---|---|
| `IsSynchronized` | Value matches the device (read or written successfully) |
| `IsDirty` | Value was changed locally and not yet written to device |
| `IsValid` | Passes all validation rules (no `ValidationResults`) |
| `IsError` | Last read/write threw an exception (see `Exception`) |
| `ReadOnly` | No writer — `CommitChanges` is a no-op |
| `HasValue` | Value has been read at least once |

### Setting descriptor

`SettingDescriptor` carries metadata about a setting:

```c#
new SettingDescriptor(
    name: "History Interval",
    valueType: typeof(ushort?),
    accessLevel: SettingAccessLevel.BASE,   // BASE, EXTENDED, or ADVANCED
    groupName: "Logging",
    description: "Interval between history records",
    unit: "seconds",
    valueRange: new ValueRange { Min = 1, Max = 3600, Step = 1 }
)
```

### Setting proxy

Use `ADeviceSettingProxy<T, TProxied>` when a setting exposed to the UI has a different type than the raw device value (e.g., expose a `TimeSpan` while the device stores a `ushort?` in seconds):

```c#
public class HistoryIntervalProxy : ADeviceSettingProxy<TimeSpan, ushort?> {
    public HistoryIntervalProxy(IDeviceSetting<ushort?> proxied, ISettingDescriptor descriptor)
        : base(proxied, descriptor) { }

    protected override TimeSpan? GetProxied() =>
        ProxiedSetting.Value.HasValue
            ? TimeSpan.FromSeconds(ProxiedSetting.Value.Value)
            : null;

    protected override ushort? CreateNewProxiedValue(ushort? current, TimeSpan value) =>
        (ushort)value.TotalSeconds;
}
```

The proxy:
- Forwards `Read`, `Reset`, and `CommitChanges` to the underlying setting
- Translates values in both directions via `GetProxied()` and `CreateNewProxiedValue()`
- Forwards all state flags (`IsSynchronized`, `IsError`, `Exception`, etc.)
- Subscribes to `PropertyChanged` on the proxied setting and re-raises events

### Access levels

```c#
public enum SettingAccessLevel {
    BASE     = 0,  // Standard user settings
    EXTENDED = 1,  // Power-user / service settings
    ADVANCED = 2   // Factory / calibration settings
}
```

Use `ISettingDescriptors` to filter which settings are visible to a particular user role. See [Device implementation](./device-implementation.md).
