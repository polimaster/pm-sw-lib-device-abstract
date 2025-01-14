using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device abstract implementation
/// </summary>
public abstract class ADevice : IDevice {

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport"/>
    public ITransport Transport { get; }

    /// <inheritdoc />
    public DeviceInfo? DeviceInfo { get; protected set; }

    /// <inheritdoc />
    public virtual string Id => Transport.Client.ConnectionId;

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
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());
    }
    

    /// <inheritdoc />
    public abstract Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken);

    /// <inheritdoc />
    public async Task ReadAllSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Reading settings for device {D}", Id);
        var ds = GetSettings();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.Read), cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task WriteAllSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing settings for device {D}", Id);
        var ds = GetSettings();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.CommitChanges), cancellationToken);
        }
    }

    /// <summary>
    /// Execute method on device setting <see cref="IDeviceSetting{T}"/>
    /// </summary>
    /// <param name="info">Target property</param>
    /// <param name="methodName">Method to execute</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    private async Task InvokeSettingsMethod(PropertyInfo info, string methodName, CancellationToken cancellationToken) {
        var method = info.PropertyType.GetMethod(methodName);
        var setting = info.GetValue(this);
        if (setting == null || method == null) return;

        // dynamic awaitable = m?.Invoke(setting, null) ?? throw new InvalidOperationException();
        // if (awaitable != null) await awaitable;
        var p = new object[1];
        p[0] = cancellationToken;
        var task = (Task)method.Invoke(setting, p);
        if (task != null) await task;
    }

    /// <inheritdoc />
    public IEnumerable<PropertyInfo> GetSettings() {
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
        Transport.Dispose();
    }
}