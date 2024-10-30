using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;

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
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting, ISettingBehaviour? settingBehaviour = null) {
        ProxiedSetting = proxiedSetting;
        Behaviour = settingBehaviour ?? new SettingBehaviourBase();
    }

    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    protected IDeviceSetting<TProxied> ProxiedSetting { get; }

    /// <inheritdoc />
    public ISettingBehaviour? Behaviour { get; }

    /// <inheritdoc />
    public bool ReadOnly => ProxiedSetting.ReadOnly;

    /// <summary>
    /// Stores value while validation does not pass
    /// See <see cref="Value"/>
    /// </summary>
    private T? _internalValue;
    
    /// <inheritdoc />
    public virtual T? Value {
        get => _internalValue ?? GetProxied();
        set {
            if (!IsSynchronized) throw new Exception($"{nameof(ProxiedSetting)} should be read from device before assigning value");
            Validate(value);
            // does not allow to change proxied value until is valid
            if (ValidationErrors == null || !ValidationErrors.Any()) {
                SetProxied(value);
                _internalValue = default;
                return;
            }
            _internalValue = value;
        }
    }

    /// <inheritdoc />
    public bool IsDirty => _internalValue != null || ProxiedSetting.IsDirty;

    /// <inheritdoc />
    public bool IsSynchronized => ProxiedSetting.IsSynchronized;

    /// <inheritdoc />
    public bool IsValid => (ValidationErrors == null || !ValidationErrors.Any()) && ProxiedSetting.IsValid;

    /// <inheritdoc />
    public bool IsError => ProxiedSetting.IsError;

    /// <inheritdoc />
    public IEnumerable<ValidationResult>? ValidationErrors { get; protected set; }

    /// <inheritdoc />
    public Exception? Exception => ProxiedSetting.Exception;

    /// <summary>
    /// Converts <see cref="ProxiedSetting"/> value to <see cref="IDeviceSetting{T}.Value"/>
    /// </summary>
    /// <returns>Result of conversion</returns>
    protected abstract T? GetProxied();

    /// <summary>
    /// Converts <see cref="IDeviceSetting{T}.Value"/> to <see cref="ProxiedSetting"/> value
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <returns>Result of conversion</returns>
    protected abstract void SetProxied(T? value);
    
    /// <summary>
    /// Validates value while assignment. See <see cref="ValidationErrors"/> for errors.
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    protected virtual void Validate(T? value) {
        ValidationErrors = null;
    }
    
    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }

    /// <inheritdoc />
    public virtual Task Reset(ITransport transport, CancellationToken cancellationToken) {
        return ProxiedSetting.Reset(transport, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task CommitChanges(ITransport transport, CancellationToken cancellationToken) {
        if (ValidationErrors != null && ValidationErrors.Any()) return Task.CompletedTask;
        return ProxiedSetting.CommitChanges(transport, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task Read(ITransport transport, CancellationToken cancellationToken) {
        return ProxiedSetting.Read(transport, cancellationToken);
    }
}