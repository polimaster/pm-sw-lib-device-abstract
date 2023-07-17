using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract;

public abstract class ADeviceManager<T> : IDeviceManager<T> where T: IDevice {
    protected readonly IDeviceBuilder DeviceBuilder;
    protected readonly ILogger<ADeviceManager<T>>? Logger;
    public Action<T>? Attached  { get; set; }
    public Action<T>? Removed { get; set; }
    public List<T> Devices { get; set; } = new();

    protected ADeviceManager(IDeviceBuilder deviceBuilder, ILoggerFactory? loggerFactory = null) {
        DeviceBuilder = deviceBuilder;
        Logger = loggerFactory?.CreateLogger<ADeviceManager<T>>();
        
        Removed += dev => { Logger?.LogInformation("Device removed {D}", dev.Id); };
        Attached += dev => {
            Logger?.LogInformation("New device attached {D}", dev.Id);
        };
    }

    public virtual void Dispose() {
        foreach (var device in Devices) {
            device.Dispose();
        }
    }
}