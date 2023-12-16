> # Warning: this documentation for version 1.x.x.


# USAGE

Add the library library to your project::
> dotnet add package Polimaster.Device.Abstract

Then,

Here is quick device library implementation below. On example, name of device is _PM1703_
and it communicates with computer via _USB_ interface.

- [Setting up device transport](./setting-up-device-transport.md)
- [Device manager and Discovery](./device-manager-and-discovery.md)
- [Device implementation](./device-implementation.md)
- [Device commands and settings](./device-commands-and-settings.md)


After implementing necessary classes, you can use new library in application.

### Host

```c#
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        
        services.AddHostedService<Worker>();
        
        
        // PM1703 library
        services.AddSingleton<IPm1703DeviceManager, Pm1703MDeviceManager>();
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
    private readonly IPm1703Discovery _Discovery;
    private CancellationToken _stoppingToken;

    public Worker(ILogger<Worker> logger, IPm1703DeviceManager deviceManager, IPm1703Discovery Discovery) {
        _logger = logger;
        _deviceManager = deviceManager;
        _Discovery = Discovery;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) {
        _stoppingToken = stoppingToken;
        
        _deviceManager.Attached += DeviceManagerOnAttached;
        _deviceManager.Removed += DeviceManagerOnRemoved;

        _Discovery.Start(stoppingToken);
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken) {
        _stoppingToken = cancellationToken;
        _Discovery.Stop();
        return Task.CompletedTask;
    }
    
    private void DeviceManagerOnRemoved(IPm1703 device) {
        _logger.LogInformation("Device {M} {Mod}#{S} disconnected", device.DeviceInfo.Model, device.DeviceInfo.Modification, device.DeviceInfo.Serial);
    }

    private async void DeviceManagerOnAttached(IPm1703 device) {

        try {
            await device.ReadDeviceInfo(_stoppingToken);
            _logger.LogInformation("Found device {M} {Mod}#{S}, ID: {F}", device.DeviceInfo.Model, device.DeviceInfo.Modification,
                device.DeviceInfo.Serial, device.DeviceInfo.Id);

            await device.RefreshBatteryStatus(_stoppingToken);
            _logger.LogInformation("Battery: {V}V, {P}%", device.BatteryStatus.Volts, device.BatteryStatus.Percents);

            var deviceTime = await device.GetTime(_stoppingToken);
            _logger.LogInformation("Time on device: {V}", deviceTime);

            _logger.LogInformation("Writing current time to device");
            await device.SetTime(_stoppingToken);

            _logger.LogInformation("Reading device settings...");
            await device.ReadSettings(_stoppingToken);

            var deviceSettings = device.GetDeviceSettingsProperties();
            foreach (var setting in deviceSettings) {
                _logger.LogInformation("{N} .............. {V}", setting.Name, setting.GetValue(device)?.ToString());
            }

        } catch (Exception e) {
            _logger.LogError(e, "Fatal error");
        } finally {
            await device.Transport.Close();
        }
    }
}
```