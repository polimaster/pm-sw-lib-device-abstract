using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
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
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting) {
        ProxiedSetting = proxiedSetting;
    }

    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    protected IDeviceSetting<TProxied> ProxiedSetting { get; }

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
    public virtual async Task CommitChanges(ITransport transport, CancellationToken cancellationToken) {
        if (ValidationErrors != null && ValidationErrors.Any()) return;
        await ProxiedSetting.CommitChanges(transport, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task Read(ITransport transport, CancellationToken cancellationToken) {
        // check if proxied setting already had read
        if (ProxiedSetting.Value == null) await ProxiedSetting.Read(transport, cancellationToken);
    }
}