using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Proxied device setting. Converts underlying <see cref="IDeviceSetting{T}"/> value to its own.
/// Usually, its required when device returns structured value like byte masks or complex strings.
/// </summary>
public abstract class ADeviceSettingProxy<T, TProxied> : ADeviceSettingBase<T>, IDeviceSetting<T> where T : notnull where TProxied : notnull {
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="proxiedSetting">Setting to proxy</param>
    /// <param name="settingBehaviour">See <see cref="ISettingBehaviour"/></param>
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting, ISettingBehaviour? settingBehaviour = null) :
        base(settingBehaviour) {
        ProxiedSetting = proxiedSetting;
    }

    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    protected IDeviceSetting<TProxied> ProxiedSetting { get; }

    /// <inheritdoc />
    public override bool ReadOnly => ProxiedSetting.ReadOnly;

    /// <inheritdoc />
    public override T? Value {
        get => base.HasValue ? base.Value : GetProxied();
        set {
            if (!ProxiedSetting.HasValue)
                throw new Exception($"Underlying {ProxiedSetting.GetType().Name} should be read from device before assigning value");
            base.Value = value;
            // does not allow to change proxied value until is valid
            if (!ValidationErrors.Any()){
                ProxiedSetting.Value = ModifyProxied(ProxiedSetting.Value ?? throw new InvalidOperationException(),
                    value ?? throw new ArgumentNullException(nameof(value)));
            }
        }
    }

    /// <inheritdoc />
    public override bool HasValue {
        get => base.HasValue || ProxiedSetting.HasValue;
        protected set => base.HasValue = value;
    }

    /// <inheritdoc />
    public override bool IsDirty => base.HasValue || ProxiedSetting.IsDirty;

    /// <inheritdoc />
    public override bool IsSynchronized => ProxiedSetting.IsSynchronized;

    /// <inheritdoc />
    public override bool IsValid => !ValidationErrors.Any() && ProxiedSetting.IsValid;

    /// <inheritdoc />
    public override bool IsError => ProxiedSetting.IsError;

    /// <inheritdoc />
    public override Exception? Exception {
        get => ProxiedSetting.Exception;
        protected set { }
    }

    /// <summary>
    /// Converts <see cref="ProxiedSetting"/> value to <see cref="Value"/>
    /// </summary>
    /// <returns>Result of conversion</returns>
    protected abstract T? GetProxied();

    /// <summary>
    /// Apply changes to <see cref="ProxiedSetting"/>.<see cref="IDeviceSetting{T}.Value"/>
    /// </summary>
    /// <param name="proxied">Current proxied value to modify</param>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <returns>Result of modification</returns>
    protected abstract TProxied ModifyProxied(TProxied proxied, T value);

    /// <inheritdoc />
    public override Task Reset(CancellationToken cancellationToken) {
        HasValue = false;
        return ProxiedSetting.Reset(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!ValidationErrors.Any()) {
            await ProxiedSetting.CommitChanges(cancellationToken);
            return;
        }

        Exception = new Exception($"Value of {GetType().Name} is not valid");
    }

    /// <inheritdoc />
    public override async Task Read(CancellationToken cancellationToken) {
        HasValue = false;
        await ProxiedSetting.Read(cancellationToken);
    }
}