### Define device discovery
Discovery searches for devices on particular interface (like IrDA or Bluetooth).

```c#
public interface IPm1703MDiscovery : ITransportDiscovery<UsbDeviceParams> {
}

public class Pm1703Discovery : ATransportDiscovery<UsbDeviceParams>, IPm1703Discovery {
    private readonly ManagementEventWatcher _managementEventWatcher;
    private const string VID = "VID_2047";
    private const string PID = "PID_0A1B";
    private readonly ILogger<Pm1703Discovery>? _logger;
    private CancellationTokenSource? _watchTokenSource;
    private const int TIMEOUT = 20;

    public Pm1703Discovery(IClientFactory factory, ILoggerFactory? loggerFactory = null) :
        base(factory, loggerFactory) {
        _logger = loggerFactory?.CreateLogger<Pm1703Discovery>();

        const string query =
            "SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance isa 'Win32_SerialPort'";
        _managementEventWatcher = new ManagementEventWatcher(new WqlEventQuery(query));
        _managementEventWatcher.EventArrived += OnMEWEvent;
    }

    public override void Start(CancellationToken token) {
        _logger?.LogDebug("Starting ManagementEventWatcher");
        _watchTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);
        _managementEventWatcher.Start();

        Task.Run(() => {
            while (true) {
                if (_watchTokenSource.Token.IsCancellationRequested) {
                    _managementEventWatcher.Stop();
                    break;
                }
                Thread.Sleep(TIMEOUT);
            }
            
        }, _watchTokenSource.Token);

        _logger?.LogDebug("Search for already connected devices");
        using var searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_SerialPort");
        var managementObjectCollection = searcher.Get();

        foreach (var deviceUsb in managementObjectCollection) {
            var pnpDeviceId = (string)deviceUsb.GetPropertyValue("PNPDeviceID");
            var deviceId = (string)deviceUsb.GetPropertyValue("DeviceID");
            if (pnpDeviceId.Contains($"{VID}&{PID}")) {
                OnFound(new UsbDeviceParams { Name = deviceId, PlugAndPlayId = pnpDeviceId });
            }
        }
    }

    public override void Stop() {
        _logger?.LogDebug("Stopping device discovery");
        _watchTokenSource?.Cancel();
    }

    private void OnMEWEvent(object sender, EventArrivedEventArgs e) {
        var p = e.NewEvent.Properties["TargetInstance"];
        if (p.Value is not ManagementBaseObject mbo) return;

        var deviceId = mbo.Properties["DeviceID"];
        var pnpDeviceId = mbo.Properties["PNPDeviceID"];

        var usbDevice = new UsbDeviceParams
            { Name = deviceId.Value.ToString(), PlugAndPlayId = pnpDeviceId.Value.ToString() };
        if (!usbDevice.PlugAndPlayId.Contains($"{VID}&{PID}")) return;

        switch (e.NewEvent.ClassPath.ClassName) {
            case "__InstanceCreationEvent":
                OnFound(usbDevice);
                break;
            case "__InstanceDeletionEvent":
                OnLost(usbDevice);
                break;
        }
    }

    private void OnFound(UsbDeviceParams usbDevice) {
        _logger?.LogDebug("Found device {P}:{A}", usbDevice.Name, usbDevice.PlugAndPlayId);
        var client = ClientFactory.CreateClient<IUsbClient, UsbDevice>();
        var res = new Pm1703Transport(client, usbDevice, LoggerFactory);
        Found?.Invoke(new List<ITransport<UsbDeviceParams>> { res });
    }

    private void OnLost(UsbDeviceParams usbDevice) {
        _logger?.LogDebug("Lost device {P}:{A}", usbDevice.Name, usbDevice.PlugAndPlayId);
        var client = ClientFactory.CreateClient<IUsbClient, UsbDeviceParams>();
        var res = new Pm1703Transport(client, usbDevice, LoggerFactory);
        Lost?.Invoke(new List<ITransport<UsbDeviceParams>> { res });
    }

    public override void Dispose() {
        _managementEventWatcher.EventArrived -= OnMEWEvent;
        _managementEventWatcher.Stop();
        _managementEventWatcher.Dispose();
    }
}
```

### Define device manager

```c#
public interface IPm1703DeviceManager : IDeviceManager<IPm1703> {
}

public class Pm1703DeviceManager : ADeviceManager<IPm1703>, IPm1703DeviceManager {
    private readonly IPm1703Discovery _discovery;

    public Pm1703DeviceManager(IPm1703Discovery discovery, IDeviceBuilder deviceBuilder,
        ILoggerFactory? loggerFactory = null) : base(
        deviceBuilder, loggerFactory) {
        _discovery = discovery;
        _discovery.Found += OnFound;
        _discovery.Lost += OnLost;
    }

    private void OnLost(IEnumerable<ITransport<UsbDevice>> transports) {
        
        void Removed(IPm1703 dev) {
            this.Removed?.Invoke(dev);
            dev.Dispose();
        }
        
        var discovered = from transport in transports select DeviceBuilder.With(transport).Build<Pm1703>();
        var toRemove = Devices.Where(x => discovered.All(y => y != x)).ToList();
        foreach (var dev in toRemove) Removed(dev);
    }

    private void OnFound(IEnumerable<ITransport<UsbDevice>> transports) {
        var discovered = from transport in transports select DeviceBuilder.With(transport).Build<Pm1703>();
        var devices = discovered.ToList();
        Devices.AddRange(devices);
        foreach (var device in devices) { Attached?.Invoke(device); }
    }

    public override void Dispose() {
        base.Dispose();
        _discovery.Dispose();
    }
}

```