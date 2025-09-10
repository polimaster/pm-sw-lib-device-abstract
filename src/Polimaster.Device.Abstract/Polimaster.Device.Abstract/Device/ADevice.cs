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
public abstract class ADevice<TTransport, TStream> : IDevice<TTransport, TStream>
    where TTransport : ITransport<TStream> {
    /// <summary>
    /// Settings descriptors for device type
    /// </summary>
    private readonly IEnumerable<ISettingDescriptor> _settingDescriptors;

    /// <summary>
    /// Settings properties cache
    /// </summary>
    private List<PropertyInfo>? _settingsProperties;

    /// <summary>
    /// Transport layer
    /// </summary>
    /// <see cref="ITransport{TStream}"/>
    protected TTransport Transport { get; }

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
    /// <param name="settingDescriptors"></param>
    /// <param name="loggerFactory">Logger factory</param>
    protected ADevice(TTransport transport, ISettingDescriptors settingDescriptors, ILoggerFactory? loggerFactory = null) {
        _settingDescriptors = settingDescriptors.GetAll();
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());
    }


    /// <inheritdoc />
    public abstract Task<DeviceInfo?> ReadDeviceInfo(CancellationToken cancellationToken);

    /// <inheritdoc />
    public async Task ReadAllSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Reading settings for device {D}", Id);
        var ds = GetSettingsProperties();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting.Read), cancellationToken);
        }
    }

    /// <inheritdoc />
    public async Task WriteAllSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing settings for device {D}", Id);
        var ds = GetSettingsProperties();
        foreach (var info in ds) {
            if (cancellationToken.IsCancellationRequested) return;
            await InvokeSettingsMethod(info, nameof(IDeviceSetting.CommitChanges), cancellationToken);
        }
    }

    /// <inheritdoc />
    public IDeviceSetting SetSetting<T>(ISettingDescriptor descriptor, T value) where T : notnull {
        if (descriptor.ValueType != typeof(T))
            throw new ArgumentException($"Type of {nameof(value)} must be of type {descriptor.ValueType}");

        var setting = GetSetting(descriptor);
        setting.UntypedValue = value;
        return setting;
    }

    /// <inheritdoc />
    public IDeviceSetting GetSetting(ISettingDescriptor descriptor) {
        var settings = GetSettings();
        return settings.FirstOrDefault(e => e.Descriptor.Equals(descriptor)) ??
               throw new ArgumentException($"No settings found for '{descriptor.Name}'");
    }

    /// <inheritdoc />
    public IEnumerable<IDeviceSetting> GetSettings() {
        var ds = GetSettingsProperties();
        var res =  new List<IDeviceSetting>();
        foreach (var info in ds) {
            if (info.GetValue(this) is not IDeviceSetting settingInstance) continue;
            if(_settingDescriptors.Contains(settingInstance.Descriptor)) res.Add(settingInstance);
        }
        return res;
    }


    /// <summary>
    /// Execute method on device setting <see cref="IDeviceSetting{T}"/>
    /// </summary>
    /// <param name="info">Target property</param>
    /// <param name="methodName">Method to execute</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    private async Task InvokeSettingsMethod(PropertyInfo info, string methodName, CancellationToken cancellationToken) {
        if (info.GetValue(this) is not IDeviceSetting setting) return;

        var method = setting.GetType().GetMethod(methodName);
        if (method == null) throw new MissingMethodException(info.Name, methodName);

        var task = (Task)method.Invoke(setting, [cancellationToken]);
        if (task != null) await task;
    }


    /// <summary>
    /// Get PropertyInfo's of all <see cref="IDeviceSetting{T}"/> instance properties.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<PropertyInfo> GetSettingsProperties() {
        if (_settingsProperties != null) return _settingsProperties;

        var propertyInfos = GetType().GetProperties();
        _settingsProperties = propertyInfos.Where(info => info.PropertyType.IsGenericType)
            .Where(info => info.PropertyType.GetGenericTypeDefinition() == typeof(IDeviceSetting<>))
            .ToList();
        return _settingsProperties;
    }

    /// <inheritdoc />
    public bool HasSame(TTransport transport) => transport.Client.Equals(Transport.Client);

    /// <inheritdoc />
    public bool Equals(IDevice<TTransport, TStream>? other) {
        return Id.Equals(other?.Id);
    }

    /// <inheritdoc />
    public void Dispose() {
        Logger?.LogDebug("Disposing device {D}", Id);
        IsDisposing?.Invoke();
        Transport.Dispose();
    }
}