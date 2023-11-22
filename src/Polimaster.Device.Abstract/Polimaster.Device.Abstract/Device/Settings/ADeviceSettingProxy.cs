using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Proxied device setting. Converts underlying <see cref="IDeviceSetting{T}"/> value to its own.
/// Usually, its required when device returns structured value like byte masks or complex strings.
/// </summary>
public abstract class ADeviceSettingProxy<T, TProxied> : IDeviceSetting<T> {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="proxiedSetting">Setting to proxy</param>
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting) {
        ProxiedSetting = proxiedSetting;
    }

    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    protected IDeviceSetting<TProxied> ProxiedSetting { get; set; }

    /// <inheritdoc />
    public bool ReadOnly => ProxiedSetting.ReadOnly;

    /// <inheritdoc />
    public virtual T? Value {
        get => FromProxied(ProxiedSetting.Value);
        set => ProxiedSetting.Value = ToProxied(value);
    }

    /// <inheritdoc />
    public bool IsDirty => ProxiedSetting.IsDirty;

    /// <inheritdoc />
    public bool IsValid => ProxiedSetting.IsValid;

    /// <inheritdoc />
    public bool IsError => ProxiedSetting.IsError;

    /// <inheritdoc />
    public IEnumerable<SettingValidationException>? ValidationErrors => ProxiedSetting.ValidationErrors;

    /// <inheritdoc />
    public Exception? Exception => ProxiedSetting.Exception;

    /// <summary>
    /// Converts <see cref="ProxiedSetting"/> value to <see cref="IDeviceSetting{T}.Value"/>
    /// </summary>
    /// <param name="value"><see cref="ProxiedSetting"/> value</param>
    /// <returns>Result of conversion</returns>
    protected abstract T? FromProxied(TProxied? value);

    /// <summary>
    /// Converts <see cref="IDeviceSetting{T}.Value"/> to <see cref="ProxiedSetting"/> value
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <returns>Result of conversion</returns>
    protected abstract TProxied? ToProxied(T? value);

    /// <inheritdoc />
    public virtual async Task CommitChanges(CancellationToken cancellationToken) => 
        await ProxiedSetting.CommitChanges(cancellationToken);

    /// <inheritdoc />
    public virtual async Task Read(CancellationToken cancellationToken) {
        if (ProxiedSetting.Value == null) await ProxiedSetting.Read(cancellationToken);
    }
}