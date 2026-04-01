# Device Implementation

## Define setting descriptors

`ISettingDescriptors` controls which settings are returned by `device.GetSettings()`. Define a concrete implementation in the device library:

```c#
public class Pm1703Descriptors : ISettingDescriptors {
    public static readonly ISettingDescriptor HistoryInterval = new SettingDescriptor(
        name: "History Interval",
        valueType: typeof(ushort?),
        groupName: "Logging",
        unit: "seconds"
    );

    public IEnumerable<ISettingDescriptor> GetAll() => [HistoryInterval];
}
```

Inject this into the device (and expose via the device manager's `SettingsDescriptors` property) so the UI layer can discover the full descriptor list independently of device instances.

## Define the device interface and class

Extend `ADevice<TTransport, TStream>` and implement `ReadDeviceInfo`. Create and initialize all settings in the constructor (or a dedicated init method).

```c#
public interface IPm1703 : IDevice<IPm1703Transport, Stream>,
    IHasBattery,
    IHasClock {
    IDeviceSetting<ushort?> HistoryInterval { get; }
}


public class Pm1703 : ADevice<IPm1703Transport, Stream>, IPm1703 {

    public BatteryStatus? BatteryStatus { get; private set; }
    public IDeviceSetting<ushort?> HistoryInterval { get; private set; }

    public Pm1703(IPm1703Transport transport, ISettingDescriptors settingDescriptors,
        ILoggerFactory? loggerFactory = null)
        : base(transport, settingDescriptors, loggerFactory) {

        HistoryInterval = new ADeviceSetting<ushort?>(new SettingDefinition<ushort?> {
            Descriptor = Pm1703Descriptors.HistoryInterval,
            Reader = new HistoryIntervalRead(transport, loggerFactory),
            Writer = new HistoryIntervalWrite(transport, loggerFactory)
        });
    }

    public override async Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken) {
        var serial = new SerialNumberRead(Transport, LoggerFactory);
        var firmware = new FirmwareVersionRead(Transport, LoggerFactory);

        DeviceInfo = new DeviceInfo {
            Model = "PM1703",
            Serial = await serial.Read(cancellationToken),
            FirmwareVersion = await firmware.Read(cancellationToken)
        };
        return DeviceInfo;
    }

    public async Task<BatteryStatus?> RefreshBatteryStatus(CancellationToken token) {
        var volts = await new BatteryVoltsRead(Transport, LoggerFactory).Read(token);
        BatteryStatus = new BatteryStatus { Volts = volts };
        return BatteryStatus;
    }

    public Task SetTime(CancellationToken cancellationToken, DateTime? dateTime = null) =>
        new TimeWrite(Transport, LoggerFactory).Write(dateTime ?? DateTime.Now, cancellationToken);

    public async Task<DateTime?> GetTime(CancellationToken cancellationToken) =>
        await new TimeRead(Transport, LoggerFactory).Read(cancellationToken);
}
```

## Device feature interfaces

Implement optional interfaces to signal capabilities to the host application:

| Interface | Members |
|---|---|
| `IHasBattery` | `BatteryStatus? BatteryStatus`, `Task<BatteryStatus?> RefreshBatteryStatus(token)` |
| `IHasClock` | `Task SetTime(token, dateTime?)`, `Task<DateTime?> GetTime(token)` |
| `IHasDose` | `Task ResetDose(token)` |
| `IHasTemperatureSensor` | `Task<double?> ReadTemperature(token)` |
| `IHasStatus<T>` | `IDeviceStatus<T> Status` |
| `IHasHistory<THistory>` | `IHistoryManager<THistory> HistoryManager`, `IDeviceSetting<TimeSpan> HistoryInterval` |

## Reading and writing settings

`ADevice` provides bulk operations via reflection — all properties of type `IDeviceSetting<>` are discovered automatically:

```c#
await device.ReadAllSettings(cancellationToken);   // calls Read() on every setting property
await device.WriteAllSettings(cancellationToken);  // calls CommitChanges() on every setting property
```

`GetSettings()` returns only settings whose `Descriptor` matches one from the injected `ISettingDescriptors`:

```c#
foreach (var setting in device.GetSettings()) {
    Console.WriteLine($"{setting.Descriptor.Name}: {setting.UntypedValue}");
}
```

Access or update a specific setting by descriptor:

```c#
var setting = device.GetSetting(Pm1703Descriptors.HistoryInterval);
device.SetSetting(Pm1703Descriptors.HistoryInterval, (ushort?)30);
```

## DeviceInfo

`DeviceInfo` is a value struct populated by `ReadDeviceInfo()`:

```c#
public struct DeviceInfo {
    public string? Model;
    public string? Modification;
    public string ModelFull => $"{Model}{Modification}";
    public string? Serial;
    public string? Id;
    public Version? HardwareVersion;
    public Version? FirmwareVersion;
    public DateTime? ManufacturingDate;
    public DateTime? CalibrationDate;
}
```

## Status monitoring (optional)

For continuous polling (e.g., radiation readings), implement `IDeviceStatus<TStatus>` by extending `ADeviceStatus<TStatus, TStream>`:

```c#
public class Pm1703Status : ADeviceStatus<RadiationStatus, Stream> {
    private CancellationTokenSource? _cts;
    public override event Action<RadiationStatus>? HasNext;

    public Pm1703Status(ITransport<Stream> transport, ILoggerFactory? loggerFactory)
        : base(transport, loggerFactory) { }

    public override async Task<RadiationStatus> Read(CancellationToken token) {
        // single read
        return await new RadiationStatusRead(Transport, LoggerFactory).Read(token);
    }

    public override void Start(CancellationToken token) {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        Task.Run(async () => {
            while (!_cts.Token.IsCancellationRequested) {
                HasNext?.Invoke(await Read(_cts.Token));
                await Task.Delay(500, _cts.Token);
            }
        }, _cts.Token);
    }

    public override void Stop() => _cts?.Cancel();
}
```

## History reading (optional)

For devices with on-board history logs, implement `IHistoryManager<THistory>` by extending `AHistoryManager<THistory, TStream>`:

```c#
public class Pm1703HistoryManager : AHistoryManager<Pm1703HistoryRecord, Stream> {
    private CancellationTokenSource? _cts;
    public override event Action<HistoryChunk<Pm1703HistoryRecord>>? HasNext;

    public Pm1703HistoryManager(ITransport<Stream> transport, ILoggerFactory? loggerFactory)
        : base(transport, loggerFactory) { }

    public override async Task Read(CancellationToken token) {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
        // read pages and fire HasNext for each chunk
    }

    public override void Stop() => _cts?.Cancel();

    public override Task Wipe(CancellationToken token) =>
        new WipeHistoryCommand(Transport, LoggerFactory).Exec(token);
}
```

`HistoryChunk<THistory>` carries:
```c#
public struct HistoryChunk<THistory> {
    public int? Remaining;    // records left (null if unknown)
    public bool Completed;    // true on the final chunk
    public IEnumerable<THistory>? Records;
    public Exception? Exception;
}
```
