using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ADeviceManager<T> : IDeviceManager<T> where T : IDevice {
    /// <summary>
    /// Device builder
    /// </summary>
    protected readonly IDeviceBuilder DeviceBuilder;

    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger<ADeviceManager<T>>? Logger;

    /// <inheritdoc />
    public Action<T>? Attached { get; set; }

    /// <inheritdoc />
    public Action<T>? Removed { get; set; }

    /// <inheritdoc />
    public List<T> Devices { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceBuilder"></param>
    /// <param name="loggerFactory"></param>
    protected ADeviceManager(IDeviceBuilder deviceBuilder, ILoggerFactory? loggerFactory = null) {
        DeviceBuilder = deviceBuilder;
        Logger = loggerFactory?.CreateLogger<ADeviceManager<T>>();

        Removed += dev => { Logger?.LogInformation("Device removed {D}", dev.Id); };
        Attached += dev => { Logger?.LogInformation("New device attached {D}", dev.Id); };
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        foreach (var device in Devices) { device.Dispose(); }
    }
}