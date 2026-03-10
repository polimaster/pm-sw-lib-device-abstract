# Device Manager and Discovery

## Define discovery

Discovery searches for devices on a particular interface (USB, IrDA, Bluetooth, etc.). Extend `ATransportDiscovery<TConnectionParams>` and call `Found` / `Lost` with connection parameter lists.

`ATransportDiscovery` runs `Search()` in a background loop every `Sleep` milliseconds (default 1000). Override `Sleep` to change the polling interval.

```c#
public interface IPm1703Discovery : ITransportDiscovery<UsbConnectionParams> {
}

public class Pm1703Discovery : ATransportDiscovery<UsbConnectionParams>, IPm1703Discovery {
    private const string VID = "VID_2047";
    private const string PID = "PID_0A1B";
    private ManagementEventWatcher? _watcher;

    public override event Action<IEnumerable<UsbConnectionParams>>? Found;
    public override event Action<IEnumerable<UsbConnectionParams>>? Lost;

    public Pm1703Discovery(ILoggerFactory? loggerFactory = null) : base(loggerFactory) { }

    public override void Start(CancellationToken token) {
        // Set up WMI watcher for plug/unplug events
        const string query =
            "SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance isa 'Win32_SerialPort'";
        _watcher = new ManagementEventWatcher(new WqlEventQuery(query));
        _watcher.EventArrived += OnWmiEvent;
        _watcher.Start();

        // Enumerate devices already connected
        base.Start(token);
    }

    protected override void Search() {
        using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SerialPort");
        var found = new List<UsbConnectionParams>();
        foreach (var obj in searcher.Get()) {
            var pnp = (string)obj["PNPDeviceID"];
            var id = (string)obj["DeviceID"];
            if (pnp.Contains($"{VID}&{PID}"))
                found.Add(new UsbConnectionParams { Name = id, PlugAndPlayId = pnp });
        }
        if (found.Count > 0) Found?.Invoke(found);
    }

    private void OnWmiEvent(object sender, EventArrivedEventArgs e) {
        if (e.NewEvent.Properties["TargetInstance"].Value is not ManagementBaseObject mbo) return;
        var param = new UsbConnectionParams {
            Name = mbo["DeviceID"]?.ToString() ?? string.Empty,
            PlugAndPlayId = mbo["PNPDeviceID"]?.ToString() ?? string.Empty
        };
        if (!param.PlugAndPlayId.Contains($"{VID}&{PID}")) return;

        switch (e.NewEvent.ClassPath.ClassName) {
            case "__InstanceCreationEvent": Found?.Invoke([param]); break;
            case "__InstanceDeletionEvent": Lost?.Invoke([param]); break;
        }
    }

    public override void Stop() {
        _watcher?.Stop();
        base.Stop();
    }

    public override void Dispose() {
        if (_watcher != null) {
            _watcher.EventArrived -= OnWmiEvent;
            _watcher.Stop();
            _watcher.Dispose();
        }
        base.Dispose();
    }
}
```

## Define a device manager

The device manager subscribes to discovery events and maintains the list of live devices. Extend `ADeviceManager<TDevice, TTransport, TStream, TDiscovery, TConnectionParams>` and implement three factory methods:

```c#
public interface IPm1703DeviceManager : IDeviceManager<IPm1703> {
}

public class Pm1703DeviceManager :
    ADeviceManager<IPm1703, IPm1703Transport, Stream, IPm1703Discovery, UsbConnectionParams>,
    IPm1703DeviceManager {

    private readonly ISettingDescriptors _descriptors = new Pm1703Descriptors();
    public override ISettingDescriptors SettingsDescriptors => _descriptors;

    public override event Action<IPm1703>? Attached;
    public override event Action<IPm1703>? Removed;

    public Pm1703DeviceManager(IPm1703Discovery discovery, ILoggerFactory? loggerFactory = null)
        : base(discovery, loggerFactory) { }

    protected override IClient<Stream> CreateClient(UsbConnectionParams connectionParams) =>
        new UsbClient(connectionParams, LoggerFactory);

    protected override IPm1703Transport CreateTransport(IClient<Stream> client) =>
        new Pm1703Transport(client, LoggerFactory);

    protected override IPm1703 CreateDevice(IPm1703Transport transport) =>
        new Pm1703(transport, SettingsDescriptors, LoggerFactory);
}
```

### What the base class does automatically

When `Discovery.Found` fires, `ADeviceManager` calls your three factory methods in sequence (`CreateClient` → `CreateTransport` → `CreateDevice`), adds the device to the internal list, and fires `Attached`.

When `Discovery.Lost` fires, it matches the incoming connection parameters to existing devices via `HasSame(transport)`, fires `Removed`, disposes the device, and removes it from the list.

The device list is protected by a lock (`Lock` object) — safe to read and mutate from multiple threads.

### Accessing devices

```c#
IReadOnlyList<IPm1703> devices = manager.GetDevices();
```

### Accessing setting descriptors

`SettingsDescriptors` is accessible directly on the manager, so the UI layer can enumerate available settings before any device connects:

```c#
foreach (var descriptor in manager.SettingsDescriptors.GetAll()) {
    Console.WriteLine($"{descriptor.GroupName} / {descriptor.Name}");
}
```
