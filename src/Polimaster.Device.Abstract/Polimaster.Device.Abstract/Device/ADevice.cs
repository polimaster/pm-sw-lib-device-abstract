using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Device.Settings;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

public abstract class ADevice<TData> : IDevice<TData> {
    protected readonly ICommandFactory<TData> CommandFactory;
    protected readonly ILogger<IDevice<TData>>? Logger;
    public DeviceInfo DeviceInfo { get; set; }
    public abstract Task<DeviceInfo> ReadDeviceInfo();
    public ITransport<TData> Transport { get; }
    public virtual string Id => Transport.ConnectionId;
    public Action? IsDisposing { get; set; }

    /// <summary>
    /// Device constructor
    /// </summary>
    /// <param name="transport"><see cref="Transport"/></param>
    /// <param name="loggerFactory"></param>
    protected ADevice(ITransport<TData> transport, ILoggerFactory? loggerFactory = null) {
        CommandFactory = new CommandFactory<TData>(transport, loggerFactory);
        Logger = loggerFactory?.CreateLogger<ADevice<TData>>();
        Transport = transport;
    }

    public void Dispose() {
        IsDisposing?.Invoke();
        Transport.Dispose();
    }
    
    public async Task ReadSettings() {
        Logger?.LogDebug("Reading settings for device {D}", Id);
        var ds = GetDeviceSettingsProperties();
        foreach (var info in ds) await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.Read));
    }
    
    public async Task WriteSettings() {
        Logger?.LogDebug("Writing settings for device {D}", Id);
        var ds = GetDeviceSettingsProperties();
        foreach (var info in ds) await InvokeSettingsMethod(info, nameof(IDeviceSetting<object>.CommitChanges));
    }

    private async Task InvokeSettingsMethod(PropertyInfo info, string methodName) {
        var method = info.PropertyType.GetMethod(methodName);
        var setting = info.GetValue(this);
        if (setting == null || method == null) return;

        // dynamic awaitable = m?.Invoke(setting, null) ?? throw new InvalidOperationException();
        // if (awaitable != null) await awaitable;
        var task = (Task)method.Invoke(setting, null);
        if (task != null) await task;
    }

    public IEnumerable<PropertyInfo> GetDeviceSettingsProperties() {
        var propertyInfos = GetType().GetProperties();
        return propertyInfos.Where(info => info.PropertyType.IsGenericType)
            .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(IDeviceSetting<>)).ToList();
    }
}