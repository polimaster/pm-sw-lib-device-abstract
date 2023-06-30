using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract;

public abstract class ADeviceManager<T> : IDeviceManager<T> where T: IDevice {
    protected readonly ILogger<ADeviceManager<T>>? Logger;
    public event Action<T>? Attached;
    public event Action<T>? Removed;
    public List<T> Devices { get; set; } = new();

    protected ADeviceManager(ILoggerFactory? loggerFactory = null) { 
        Logger = loggerFactory?.CreateLogger<ADeviceManager<T>>();
        
        Removed += dev => { Logger?.LogInformation("Device removed {D}", dev.Id); };
        Attached += dev => { Logger?.LogInformation("New device attached {D}", dev.Id); };
    }

    /// <summary>
    /// Accept discovered devices
    /// </summary>
    /// <param name="discovered"></param>
    protected void Accept(IEnumerable<T> discovered) {
        
        bool Compare(T y, T x) {
            return y.Id == x.Id;
        }
        
        void Removed(T dev) {
            this.Removed?.Invoke(dev);
            dev.Dispose();
        }
        
        var existing = Devices.Where(x => discovered.Any(y => Compare(y, x))).ToList();
        var toRemove = Devices.Where(x => !discovered.Any(y => Compare(y, x))).ToList();
        var toAdd = discovered.Where(x => !Devices.Any(y => Compare(y, x))).ToList();

        Devices = existing;
        Attach(toAdd);
        foreach (var dev in toRemove) Removed(dev);
    }

    private void Attach(IEnumerable<T> devices) {
        var collection = devices as T[] ?? devices.ToArray();
        Devices.AddRange(collection);
        foreach (var device in collection) {
            Attached?.Invoke(device);
        }
    }

    public virtual void Dispose() {
        foreach (var device in Devices) {
            device.Dispose();
        }
    }
}