using System;
using System.ComponentModel.DataAnnotations;
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
    /// <param name="settingDescriptor">See <see cref="ISettingDescriptor"/></param>
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting, ISettingDescriptor settingDescriptor) :
        base(settingDescriptor) {
        ProxiedSetting = proxiedSetting;
        ProxiedSetting.PropertyChanged += (_, args) => {
            switch (args.PropertyName) {
                case nameof(ProxiedSetting.Value):
                    OnPropertyChanged(nameof(Value));
                    break;
                case nameof(ProxiedSetting.HasValue):
                    OnPropertyChanged(nameof(HasValue));
                    break;
                case nameof(ProxiedSetting.IsSynchronized):
                    OnPropertyChanged(nameof(IsSynchronized));
                    break;
                case nameof(ProxiedSetting.IsError):
                    OnPropertyChanged(nameof(IsError));
                    break;
                case nameof(ProxiedSetting.Exception):
                    OnPropertyChanged(nameof(Exception));
                    break;
                case nameof(ProxiedSetting.IsValid):
                    OnPropertyChanged(nameof(IsValid));
                    break;
            }
        };
    }

    /// <summary>
    /// Proxied <see cref="IDeviceSetting{T}"/> 
    /// </summary>
    protected IDeviceSetting<TProxied> ProxiedSetting { get; }

    /// <inheritdoc />
    public override bool ReadOnly => ProxiedSetting.ReadOnly;

    /// <inheritdoc />
    [Required]
    public override T? Value {
        get => base.HasValue ? base.Value : GetProxied();
        set {
            if (!ProxiedSetting.HasValue)
                throw new Exception($"Underlying {ProxiedSetting.GetType().Name} should be read from device before assigning value");
            base.Value = value;
            // does not allow to change proxied value until is valid
            if (ValidationResults.Any()) return;

            ProxiedSetting.Value = ModifyProxied(ProxiedSetting.Value ?? throw new InvalidOperationException(),
                value ?? throw new ArgumentNullException(nameof(value)));

            if (!ProxiedSetting.ValidationResults.Any()) return;

            ValidationResults.Clear();
            ValidationResults.AddRange(ProxiedSetting.ValidationResults);
            OnPropertyChanged(nameof(ValidationResults));
            OnPropertyChanged(nameof(IsValid));
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
    public override bool IsValid => !ValidationResults.Any() && ProxiedSetting.IsValid;

    /// <inheritdoc />
    public override bool IsError => ProxiedSetting.IsError;

    /// <inheritdoc />
    public override Exception? Exception {
        get => base.Exception ?? ProxiedSetting.Exception;
        protected set => base.Exception = value;
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
        Exception = null;
        return ProxiedSetting.Reset(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!ValidationResults.Any()) {
            Exception = null;
            await ProxiedSetting.CommitChanges(cancellationToken);
            return;
        }

        Exception = new Exception("Value is not valid");
        OnPropertyChanged(nameof(Exception));
    }

    /// <inheritdoc />
    public override async Task Read(CancellationToken cancellationToken) {
        HasValue = false;
        Exception = null;
        await ProxiedSetting.Read(cancellationToken);
    }
}