using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Proxied device setting. Converts underlying <see cref="IDeviceSetting{T}"/> value to its own.
/// Usually, its required when device returns structured value like byte masks or complex strings.
/// </summary>
/// <typeparam name="T">Setting data type</typeparam>
/// <typeparam name="TProxied">Data type of proxied setting</typeparam>
public abstract class ADeviceSettingProxy<T, TProxied> : IDeviceSetting<T>
    where T : notnull
    where TProxied : notnull {
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
            if (!IsSynchronized)
                throw new Exception($"{nameof(ProxiedSetting)} should be read from device before assigning value");

            // does not allow to change proxied value until is valid
            Validate(value);
            if (!ValidationErrors.Any()) {
                SetProxied(value!);
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
    public bool IsValid => !ValidationErrors.Any() && ProxiedSetting.IsValid;

    /// <inheritdoc />
    public bool IsError => ProxiedSetting.IsError;

    /// <inheritdoc />
    public List<ValidationResult> ValidationErrors { get; protected set; } = [];

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
    protected abstract void SetProxied(T value);

    /// <summary>
    /// Validates value while assignment. See <see cref="ValidationErrors"/> for errors.
    /// </summary>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    protected virtual void Validate(T? value) {
        ValidationErrors = [];
        if (value == null) {
            ValidationErrors = [
                new ValidationResult("Value is null",
                    new ArgumentNullException(nameof(value), $"{nameof(value)} cannot be null."))
            ];
        }
    }

    /// <inheritdoc />
    public override string? ToString() {
        return Value != null ? Value.ToString() : null;
    }

    /// <inheritdoc />
    public virtual Task Reset(CancellationToken cancellationToken) {
        return ProxiedSetting.Reset(cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task CommitChanges(CancellationToken cancellationToken) {
        return ValidationErrors.Any() ? Task.CompletedTask : ProxiedSetting.CommitChanges(cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task Read(CancellationToken cancellationToken) {
        return ProxiedSetting.Read(cancellationToken);
    }
}