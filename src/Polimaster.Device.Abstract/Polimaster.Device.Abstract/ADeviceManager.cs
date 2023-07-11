using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract;

public abstract class ADeviceManager<T, TTransport> : IDeviceManager<T, TTransport> where T: IDevice<TTransport> {
    protected readonly IDeviceBuilder<TTransport> DeviceBuilder;
    protected readonly ILogger<ADeviceManager<T, TTransport>>? Logger;
    public Action<T>? Attached  { get; set; }
    public Action<T>? Removed { get; set; }
    public List<T> Devices { get; set; } = new();
    public abstract void StartDeviceDiscovery(CancellationToken token, int timeout = 20);
    public abstract void StopDeviceDiscovery();

    protected ADeviceManager(IDeviceBuilder<TTransport> deviceBuilder, ILoggerFactory? loggerFactory = null) {
        DeviceBuilder = deviceBuilder;
        Logger = loggerFactory?.CreateLogger<ADeviceManager<T, TTransport>>();
        
        Removed += dev => { Logger?.LogInformation("Device removed {D}", dev.Id); };
        Attached += dev => { Logger?.LogInformation("New device attached {D}", dev.Id); };
    }

    public virtual void Dispose() {
        foreach (var device in Devices) {
            device.Dispose();
        }
    }
}