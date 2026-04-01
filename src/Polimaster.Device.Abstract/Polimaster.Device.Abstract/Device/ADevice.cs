using System;
using System.Collections.Concurrent;
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
    /// Settings properties cache shared across all instances of the same type
    /// </summary>
    // ReSharper disable once StaticMemberInGenericType
    private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> SETTINGS_PROPERTIES_CACHE = new();

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
    public virtual async Task ReadAllSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Reading settings for device {D}", Id);
        foreach (var info in GetSettingsProperties()) {
            if (cancellationToken.IsCancellationRequested) return;
            if (info.GetValue(this) is IDeviceSetting setting)
                await setting.Read(cancellationToken);
        }
    }

    /// <inheritdoc />
    public virtual async Task WriteAllSettings(CancellationToken cancellationToken) {
        Logger?.LogDebug("Writing settings for device {D}", Id);
        foreach (var info in GetSettingsProperties()) {
            if (cancellationToken.IsCancellationRequested) return;
            if (info.GetValue(this) is IDeviceSetting setting)
                await setting.CommitChanges(cancellationToken);
        }
    }

    /// <inheritdoc />
    public virtual IDeviceSetting SetSetting<T>(ISettingDescriptor descriptor, T value) {
        if (descriptor.ValueType != typeof(T))
            throw new ArgumentException($"Type of {nameof(value)} must be of type {descriptor.ValueType}");

        var setting = GetSetting(descriptor);
        setting.UntypedValue = value;
        return setting;
    }

    /// <inheritdoc />
    public virtual IDeviceSetting GetSetting(ISettingDescriptor descriptor) {
        var settings = GetSettings();
        return settings.FirstOrDefault(e => e.Descriptor.Equals(descriptor)) ??
               throw new ArgumentException($"No settings found for '{descriptor.Name}'");
    }

    /// <inheritdoc />
    public virtual IEnumerable<IDeviceSetting> GetSettings() {
        var ds = GetSettingsProperties();
        var res =  new List<IDeviceSetting>();
        foreach (var info in ds) {
            if (info.GetValue(this) is not IDeviceSetting settingInstance) continue;
            if(_settingDescriptors.Contains(settingInstance.Descriptor)) res.Add(settingInstance);
        }
        return res;
    }


    /// <summary>
    /// Get PropertyInfo's of all <see cref="IDeviceSetting{T}"/> instance properties.
    /// Result is cached per concrete type.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<PropertyInfo> GetSettingsProperties() =>
        SETTINGS_PROPERTIES_CACHE.GetOrAdd(GetType(), static t =>
            t.GetProperties()
             .Where(info => info.PropertyType.IsGenericType &&
                            info.PropertyType.GetGenericTypeDefinition() == typeof(IDeviceSetting<>))
             .ToList());

    /// <inheritdoc />
    public virtual bool HasSame(TTransport transport) => transport.Client.Equals(Transport.Client);

    /// <inheritdoc />
    public virtual bool Equals(IDevice<TTransport, TStream>? other) {
        return Id.Equals(other?.Id);
    }

    /// <inheritdoc />
    public virtual bool Equals(IDevice other) {
        return Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public virtual void Dispose() {
        Logger?.LogDebug("Disposing device {D}", Id);
        IsDisposing?.Invoke();
        foreach (var info in GetSettingsProperties()) {
            if (info.GetValue(this) is IDeviceSetting setting)
                setting.Dispose();
        }
        Transport.Dispose();
    }


}