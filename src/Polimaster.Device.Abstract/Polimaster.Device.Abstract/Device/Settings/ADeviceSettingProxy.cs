using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Proxied device setting. Converts underlying <see cref="IDeviceSetting{T}"/> value to its own.
/// Usually, its required when device returns structured value like byte masks or complex strings.
/// </summary>
public abstract class ADeviceSettingProxy<T, TProxied> : ADeviceSetting<T>, IDeviceSetting<T> {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="proxiedSetting">Setting to proxy</param>
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting, ISettingBehaviour? settingBehaviour = null): base(settingBehaviour) {
        ProxiedSetting = proxiedSetting;
    }

    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    protected IDeviceSetting<TProxied> ProxiedSetting { get; }

    /// <inheritdoc />
    public override bool ReadOnly => ProxiedSetting.ReadOnly;

    /// <summary>
    /// Stores value while validation does not pass
    /// See <see cref="Value"/>
    /// </summary>
    private T? _internalValue;
    
    /// <inheritdoc />
    public override T? Value {
        get => _internalValue ?? GetProxied();
        set {
            if (!IsSynchronized) throw new Exception($"{nameof(ProxiedSetting)} should be read from device before assigning value");
            Validate(value);
            // does not allow to change proxied value until is valid
            if (!ValidationErrors.Any()) {
                SetProxied(value);
                _internalValue = default;
                return;
            }
            _internalValue = value;
        }
    }

    /// <inheritdoc />
    public override bool IsDirty => _internalValue != null || ProxiedSetting.IsDirty;

    /// <inheritdoc />
    public override bool IsSynchronized => ProxiedSetting.IsSynchronized;

    /// <inheritdoc />
    public override bool IsValid => !ValidationErrors.Any() && ProxiedSetting.IsValid;

    /// <inheritdoc />
    public override bool IsError => ProxiedSetting.IsError;

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

    /// <inheritdoc />
    public override Task Reset(CancellationToken cancellationToken) {
        return ProxiedSetting.Reset(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!ValidationErrors.Any()) {
            await ProxiedSetting.CommitChanges(cancellationToken);
            Exception = ProxiedSetting.Exception;
            return;
        }

        Exception = new Exception($"Value of {GetType().Name} is not valid");
    }

    /// <inheritdoc />
    public override async Task Read(CancellationToken cancellationToken) {
        await ProxiedSetting.Read(cancellationToken);
        Exception = ProxiedSetting.Exception;
    }
}