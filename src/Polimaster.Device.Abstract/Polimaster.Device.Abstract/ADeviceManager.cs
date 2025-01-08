using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ADeviceManager<T> : IDeviceManager<T> where T : IDevice {
    /// <summary>
    /// 
    /// </summary>
    protected readonly ILoggerFactory? LoggerFactory;

    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger? Logger;

    /// <inheritdoc />
    public abstract event Action<T>? Attached;

    /// <inheritdoc />
    public abstract event Action<T>? Removed;

    /// <inheritdoc />
    public List<T> Devices { get; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ADeviceManager(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <summary>
    /// Create new device from found transport connection
    /// </summary>
    /// <param name="transport"></param>
    /// <returns></returns>
    protected abstract T FromTransport(ITransport transport);

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing {D}", GetType().Name);
        foreach (var device in Devices) { device.Dispose(); }
    }
}

/// <inheritdoc />
public abstract class ADeviceManager<TDiscovery, TDevice> : ADeviceManager<TDevice> where TDevice: IDevice where TDiscovery : ITransportDiscovery {
    private readonly TDiscovery _discovery;

    /// <inheritdoc />
    public override event Action<TDevice>? Attached;

    /// <inheritdoc />
    public override event Action<TDevice>? Removed;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="discovery"></param>
    /// <param name="loggerFactory"></param>
    protected ADeviceManager(TDiscovery discovery, ILoggerFactory? loggerFactory) : base(loggerFactory) {
        _discovery = discovery;
        _discovery.Found += OnFound;
        _discovery.Lost += OnLost;
    }

    /// <summary>
    /// When override remove from <see cref="ADeviceManager{T}.Devices"/> list and invoke <see cref="ADeviceManager{T}.Removed"/> action.
    /// </summary>
    /// <param name="transports"></param>
    protected virtual void OnLost(IEnumerable<ITransport> transports) {
        var toRemove = Devices.Where(x => transports.Any(x.HasSame)).ToList();
        
        Devices.RemoveAll(x => toRemove.All(y => y.Equals(x)));
        foreach (var dev in toRemove) Removed(dev);
        return;

        void Removed(TDevice dev) {
            this.Removed?.Invoke(dev);
            dev.Dispose();
        }
    }

    /// <summary>
    /// When override fill <see cref="ADeviceManager{T}.Devices"/> list and invoke <see cref="ADeviceManager{T}.Attached"/> action.
    /// </summary>
    /// <param name="transports"></param>
    protected virtual void OnFound(IEnumerable<ITransport> transports) {
        foreach (var transport in transports) {
            var found = Devices.Any(x => x.HasSame(transport));
            if(found) continue;

            var dev = FromTransport(transport);
            Devices.Add(dev);
            Attached?.Invoke(dev);
        }
    }

    /// <inheritdoc />
    public override void Dispose() {
        base.Dispose();
        _discovery.Dispose();
    }
}