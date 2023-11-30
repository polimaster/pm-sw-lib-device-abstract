using System;
using System.Collections.Generic;
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
    protected readonly ILogger<ADeviceManager<T>>? Logger;

    /// <inheritdoc />
    public abstract event Action<T>? Attached;

    /// <inheritdoc />
    public abstract event Action<T>? Removed;

    /// <inheritdoc />
    public List<T> Devices { get; protected set; } = new();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ADeviceManager(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger<ADeviceManager<T>>();
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing {D}", GetType().Name);
        foreach (var device in Devices) { device.Dispose(); }
    }
}

/// <inheritdoc />
public abstract class ADeviceManager<TDiscovery, TDevice> : ADeviceManager<TDevice> where TDevice: IDevice where TDiscovery : ITransportDiscovery {
    private readonly TDiscovery _discovery;

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
    /// When override fill <see cref="ADeviceManager{T}.Devices"/> list and invoke <see cref="ADeviceManager{T}.Attached"/> action.
    /// </summary>
    /// <param name="transports"></param>
    protected abstract void OnLost(IEnumerable<ITransport> transports);

    /// <summary>
    /// When override remove from <see cref="ADeviceManager{T}.Devices"/> list and invoke <see cref="ADeviceManager{T}.Removed"/> action.
    /// </summary>
    /// <param name="transports"></param>
    protected abstract void OnFound(IEnumerable<ITransport> transports);

    /// <inheritdoc />
    public override void Dispose() {
        base.Dispose();
        _discovery.Dispose();
    }
}