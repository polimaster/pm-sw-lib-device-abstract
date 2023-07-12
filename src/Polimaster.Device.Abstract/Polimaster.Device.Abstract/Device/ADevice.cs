using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

/// <summary>
/// Device abstract implementation
/// </summary>
/// <inheritdoc cref="IDevice{T}"/>
public abstract class ADevice<T> : IDevice<T> {
    public ICommandBuilder<T> CommandBuilder { get; set; } = null!;
    public ISettingBuilder SettingBuilder { get; set; } = null!;
    public ITransport<T> Transport { get; set; } = null!;

    public DeviceInfo DeviceInfo { get; protected set; }
    
    public virtual string Id => Transport.ConnectionId;
    public Action? IsDisposing { get; set; }
    public ILogger<IDevice>? Logger { get; set; }

    public abstract Task<DeviceInfo> ReadDeviceInfo(CancellationToken cancellationToken = new());
    
    public void Dispose() {
        IsDisposing?.Invoke();
        Transport.Dispose();
    }

    public async Task ReadSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Reading settings for device {D}", Id);
        var ds = GetDeviceSettingsProperties();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.Read), cancellationToken);
        }
    }

    public async Task WriteSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing settings for device {D}", Id);
        var ds = GetDeviceSettingsProperties();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.CommitChanges), cancellationToken);
        }
    }

    public abstract void BuildSettings();

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

    public IEnumerable<PropertyInfo> GetDeviceSettingsProperties() {
        var propertyInfos = GetType().GetProperties();
        return propertyInfos.Where(info => info.PropertyType.IsGenericType)
            .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(IDeviceSetting<>))
            .ToList();
    }

    public bool Equals(IDevice other) {
        return Id == other.Id;
    }
}