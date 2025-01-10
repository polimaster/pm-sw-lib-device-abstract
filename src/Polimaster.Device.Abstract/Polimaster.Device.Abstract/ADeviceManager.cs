using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ADeviceManager<T> : IDeviceManager<T> where T : IDisposable {
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
    public List<T> Devices { get; } = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ADeviceManager(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing {D}", GetType().Name);
        foreach (var device in Devices) { device.Dispose(); }
    }
}


/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="TDevice"><see cref="IDevice{T}"/></typeparam>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TDiscovery"><see cref="ITransportDiscovery{TConnectionParams}"/></typeparam>
/// <typeparam name="TConnectionParams"></typeparam>
public abstract class ADeviceManager<TDevice, T, TDiscovery, TConnectionParams> :
    ADeviceManager<TDevice> where TDevice: IDevice<T> where TDiscovery : ITransportDiscovery<TConnectionParams> {
    /// <summary>
    /// See <see cref="ITransportDiscovery{TConnectionParams}"/>
    /// </summary>
    protected readonly TDiscovery Discovery;

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
        Discovery = discovery;
        Discovery.Found += OnFound;
        Discovery.Lost += OnLost;
    }

    /// <summary>
    /// Create new device from found transport connection
    /// </summary>
    /// <param name="transport"></param>
    /// <returns></returns>
    protected abstract TDevice CreateDevice(ITransport<T> transport);

    /// <summary>
    /// Create new transport connection
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    protected abstract ITransport<T> CreateTransport(IClient<T> client);

    /// <summary>
    /// Create new client from <see cref="TConnectionParams"/>
    /// </summary>
    /// <param name="connectionParams"></param>
    /// <returns></returns>
    protected abstract IClient<T> CreateClient(TConnectionParams connectionParams);

    /// <summary>
    /// When override remove from <see cref="ADeviceManager{T}.Devices"/> list and invoke <see cref="ADeviceManager{T}.Removed"/> action.
    /// </summary>
    /// <param name="parameters"></param>
    protected virtual void OnLost(IEnumerable<TConnectionParams> parameters) {
        var toRemove = Devices.Where(x =>
            parameters.Any(p => x.Transport.Client.Equals(CreateClient(p)))).ToArray();
        foreach (var dev in toRemove) Removed(dev);

        Devices.RemoveAll(x => toRemove.Any(y => y.Equals(x)));

        return;

        void Removed(TDevice dev) {
            this.Removed?.Invoke(dev);
            dev.Dispose();
        }
    }

    /// <summary>
    /// When override fill <see cref="ADeviceManager{T}.Devices"/> list and invoke <see cref="ADeviceManager{T}.Attached"/> action.
    /// </summary>
    /// <param name="parameters"></param>
    protected virtual void OnFound(IEnumerable<TConnectionParams> parameters) {
        foreach (var parameter in parameters) {
            var client = CreateClient(parameter);
            var found = Devices.Any(x => x.Transport.Client.Equals(client));
            if(found) continue;

            var transport = CreateTransport(client);
            var dev = CreateDevice(transport);
            Devices.Add(dev);
            Attached?.Invoke(dev);
        }
    }

    /// <inheritdoc />
    public override void Dispose() {
        base.Dispose();
        Discovery.Dispose();
    }
}