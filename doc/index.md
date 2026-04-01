# USAGE

Add the library to your project:
```
dotnet add package Polimaster.Device.Abstract
```

Here is a quick device library implementation example. The device is named _PM1703_ and communicates via _USB_.

- [Setting up device transport](./setting-up-device-transport.md)
- [Device manager and Discovery](./device-manager-and-discovery.md)
- [Device implementation](./device-implementation.md)
- [Device commands and settings](./device-commands-and-settings.md)


After implementing the necessary classes, use the library in your application.

### Host

```c#
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {

        services.AddHostedService<Worker>();

        // PM1703 library
        services.AddSingleton<IPm1703DeviceManager, Pm1703DeviceManager>();
        services.AddSingleton<IPm1703Discovery, Pm1703Discovery>();

    })
    .UseSerilog()
    .Build();

host.Run();
```


### Worker

```c#
public class Worker : BackgroundService {
    private readonly ILogger<Worker> _logger;
    private readonly IPm1703DeviceManager _deviceManager;
    private readonly IPm1703Discovery _discovery;
    private CancellationToken _stoppingToken;

    public Worker(ILogger<Worker> logger, IPm1703DeviceManager deviceManager, IPm1703Discovery discovery) {
        _logger = logger;
        _deviceManager = deviceManager;
        _discovery = discovery;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        _stoppingToken = stoppingToken;

        _deviceManager.Attached += DeviceManagerOnAttached;
        _deviceManager.Removed += DeviceManagerOnRemoved;

        _discovery.Start(stoppingToken);
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken) {
        _discovery.Stop();
        return Task.CompletedTask;
    }

    private void DeviceManagerOnRemoved(IPm1703 device) {
        _logger.LogInformation("Device {S} disconnected", device.DeviceInfo?.Serial);
    }

    private async void DeviceManagerOnAttached(IPm1703 device) {
        try {
            await device.ReadDeviceInfo(_stoppingToken);
            _logger.LogInformation("Found device {M} #{S}", device.DeviceInfo?.Model, device.DeviceInfo?.Serial);

            await device.RefreshBatteryStatus(_stoppingToken);
            _logger.LogInformation("Battery: {V}V, {P}%", device.BatteryStatus?.Volts, device.BatteryStatus?.Percents);

            var deviceTime = await device.GetTime(_stoppingToken);
            _logger.LogInformation("Time on device: {V}", deviceTime);

            _logger.LogInformation("Writing current time to device");
            await device.SetTime(_stoppingToken);

            _logger.LogInformation("Reading device settings...");
            await device.ReadAllSettings(_stoppingToken);

            foreach (var setting in device.GetSettings()) {
                _logger.LogInformation("{N} .............. {V}", setting.Descriptor.Name, setting.UntypedValue);
            }

        } catch (Exception e) {
            _logger.LogError(e, "Fatal error");
        } finally {
            device.Dispose();
        }
    }
}
```
