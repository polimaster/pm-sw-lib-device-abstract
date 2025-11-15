using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract;

/// <inheritdoc />
public abstract class ADeviceManager<T> : IDeviceManager<T> where T : IDisposable, IDevice {
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

    /// <summary>
    /// Current connected devices
    /// </summary>
    private readonly List<T> _devices = [];

    /// <summary>
    /// Lock for <see cref="_devices"/> list manupulation
    /// </summary>
    private readonly object _devicesLock = new();

    /// <inheritdoc />
    public abstract ISettingDescriptors SettingsDescriptors { get; }

    /// <summary>
    /// Add device
    /// </summary>
    /// <param name="device"></param>
    protected void AddDevice(T device) {
        lock (_devicesLock) {
            _devices.Add(device);
        }
    }

    /// <summary>
    /// Remove device
    /// </summary>
    /// <param name="device"></param>
    protected void RemoveDevice(T device) {
        lock (_devicesLock) {
            _devices.Remove(device);
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<T> GetDevices() {
        lock (_devicesLock) return _devices.ToList(); // thread safe list
    }

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
        lock (_devicesLock) {
            foreach (var device in _devices) { device.Dispose(); }
            _devices.Clear();
        }
    }
}

/// <summary>
/// Device manager
/// </summary>
/// <typeparam name="TDiscovery"><see cref="ITransportDiscovery{TConnectionParams}"/></typeparam>
/// <typeparam name="TConnectionParams">Connection parameters for <see cref="ITransportDiscovery{TConnectionParams}"/></typeparam>
/// <typeparam name="TDevice">Device type</typeparam>
/// <typeparam name="TTransport">Transport type</typeparam>
/// <typeparam name="TStream"></typeparam>
public abstract class ADeviceManager<TDevice, TTransport, TStream, TDiscovery, TConnectionParams> :
    ADeviceManager<TDevice>
    where TDiscovery : ITransportDiscovery<TConnectionParams>
    where TDevice : IDevice<TTransport, TStream>, IDisposable
    where TTransport : ITransport<TStream> {
    /// <summary>
    /// See <see cref="ITransportDiscovery{TConnectionParams}"/>
    /// </summary>
    protected readonly TDiscovery Discovery;

    /// <summary>
    /// Thread race lock for <see cref="OnFound"/> and <see cref="OnLost"/> methods
    /// </summary>
    protected readonly object DevicesRaceLock = new();

    /// <inheritdoc />
    public override event Action<TDevice>? Attached;

    /// <inheritdoc />
    public override event Action<TDevice>? Removed;

    /// <summary>
    /// Constructor
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
    /// <param name="transport"><see cref="ITransport{TStream}"/></param>
    protected abstract TDevice CreateDevice(TTransport transport);

    /// <summary>
    /// Create a new transport connection
    /// </summary>
    /// <param name="client"></param>
    /// <returns><see cref="ITransport{TStream}"/></returns>
    protected abstract TTransport CreateTransport(IClient<TStream> client);

    /// <summary>
    /// Create a new client from parameters
    /// </summary>
    /// <param name="connectionParams">Client connection parameters</param>
    /// <returns><see cref="IClient{TStream}"/></returns>
    protected abstract IClient<TStream> CreateClient(TConnectionParams connectionParams);

    /// <summary>
    /// When override call <see cref="ADeviceManager{T}.RemoveDevice"/> list and invoke <see cref="ADeviceManager{T}.Removed"/> action.
    /// </summary>
    /// <param name="parameters"></param>
    protected virtual void OnLost(IEnumerable<TConnectionParams> parameters) {
        List<TDevice> removed = [];

        lock (DevicesRaceLock) {
            var toRemove = GetDevices().Where(x => parameters.Any(p =>
                        x.HasSame(CreateTransport(CreateClient(p))))).ToList();

            foreach (var dev in toRemove) {
                RemoveDevice(dev);
                removed.Add(dev);
            }
        }
        foreach (var dev in removed) {
            Logger?.LogDebug("Device lost: {D}", dev.Id);
            InvokeDeviceRemoved(dev);
            dev.Dispose();
        }
    }

    /// <summary>
    /// When override call <see cref="ADeviceManager{T}.AddDevice"/> list and invoke <see cref="ADeviceManager{T}.Attached"/> action.
    /// </summary>
    /// <param name="parameters"></param>
    protected virtual void OnFound(IEnumerable<TConnectionParams> parameters) {
        List<TDevice> added = [];

        lock (DevicesRaceLock) {
            foreach (var parameter in parameters) {
                try {
                    var client = CreateClient(parameter);
                    var transport = CreateTransport(client);

                    var exists = GetDevices().Any(x => x.HasSame(transport));
                    if (exists) continue;

                    var dev = CreateDevice(transport);
                    AddDevice(dev);
                    added.Add(dev);
                } catch (Exception e) {
                    Logger?.LogError(e, "Error while creating device form {Param}", parameter);
                }
            }
        }

        foreach (var dev in added) {
            Logger?.LogDebug("Device found: {D}", dev.Id);
            InvokeDeviceAttached(dev);
        }
    }

    /// <summary>
    /// Invoke event when device attached
    /// </summary>
    /// <param name="dev"></param>
    protected void InvokeDeviceAttached(TDevice dev) => Attached?.Invoke(dev);

    /// <summary>
    /// Invoke event when device removed
    /// </summary>
    /// <param name="dev"></param>
    protected void InvokeDeviceRemoved(TDevice dev) => Removed?.Invoke(dev);

    /// <inheritdoc />
    public override void Dispose() {
        base.Dispose();
        Discovery.Found -= OnFound;
        Discovery.Lost -= OnLost;
        Discovery.Dispose();
    }
}