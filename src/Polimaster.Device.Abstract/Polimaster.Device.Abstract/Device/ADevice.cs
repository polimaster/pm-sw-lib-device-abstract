using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device abstract implementation
/// </summary>
/// <inheritdoc cref="IDevice"/>
public abstract class ADevice : IDevice {

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport"/>
    private readonly ITransport _transport;

    /// <inheritdoc />
    public DeviceInfo? DeviceInfo { get; protected set; }

    /// <inheritdoc />
    public virtual string Id => _transport.ConnectionId;

    /// <inheritdoc />
    public event Action? IsDisposing;
    
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger? Logger { get; }
    
    /// <summary>
    /// Device constructor
    /// </summary>
    /// <param name="transport">Device transport layer</param>
    /// <param name="loggerFactory">Logger factory</param>
    protected ADevice(ITransport transport, ILoggerFactory? loggerFactory = null) {
        _transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());
    }
    

    /// <inheritdoc />
    public abstract Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken = new());

    /// <inheritdoc />
    public async Task ReadAllSettings(CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Reading settings for device {D}", Id);
        var ds = GetDeviceSettingsProperties();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.Read), cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task WriteAllSettings(CancellationToken cancellationToken = new()) {
        Logger?.LogDebug("Writing settings for device {D}", Id);
        var ds = GetDeviceSettingsProperties();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.CommitChanges), cancellationToken);
        }
    }

    /// <inheritdoc />
    public virtual async Task Execute(Func<ITransport, Task> action) {
        await _transport.OpenAsync();
        await action.Invoke(_transport);
        _transport.Close();
    }

    private async Task InvokeSettingsMethod(PropertyInfo info, string methodName, CancellationToken cancellationToken) {
        var method = info.PropertyType.GetMethod(methodName);
        var setting = info.GetValue(this);
        if (setting == null || method == null) return;

        // dynamic awaitable = m?.Invoke(setting, null) ?? throw new InvalidOperationException();
        // if (awaitable != null) await awaitable;
        var p = new object[2];
        p[0] = _transport;
        p[1] = cancellationToken;
        var task = (Task)method.Invoke(setting, p);
        if (task != null) await task;
    }
    
    private IEnumerable<PropertyInfo> GetDeviceSettingsProperties() {
        var propertyInfos = GetType().GetProperties();
        return propertyInfos.Where(info => info.PropertyType.IsGenericType)
            .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(IDeviceSetting<>))
            .ToList();
    }

    /// <inheritdoc />
    public bool Equals(IDevice other) {
        return Id.Equals(other.Id);
    }
    
    /// <inheritdoc />
    public void Dispose() {
        Logger?.LogDebug("Disposing device {D}", Id);
        IsDisposing?.Invoke();
        _transport.Dispose();
    }
}