using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <summary>
/// Proxied device setting. Converts underlying <see cref="IDeviceSetting{T}"/> value to its own.
/// Usually, its required when device returns structured value like byte masks or complex strings.
/// </summary>
public abstract class ADeviceSettingProxy<T, TProxied> : ADeviceSettingBase<T>, IDeviceSetting<T> {
    /// <summary>
    /// Handles property change notifications from the proxied <see cref="IDeviceSetting{TProxied}"/> instance.
    /// This delegate is attached to the <see cref="INotifyPropertyChanged.PropertyChanged"/> event of the proxied setting
    /// to propagate changes and maintain synchronization between the proxied and parent settings.
    /// </summary>
    private readonly PropertyChangedEventHandler _proxiedPropertyChanged;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="proxiedSetting">Setting to proxy</param>
    /// <param name="settingDescriptor">See <see cref="ISettingDescriptor"/></param>
    protected ADeviceSettingProxy(IDeviceSetting<TProxied> proxiedSetting, ISettingDescriptor settingDescriptor) :
        base(settingDescriptor) {
        ProxiedSetting = proxiedSetting;
        _proxiedPropertyChanged = OnProxiedSettingPropertyChanged;
        ProxiedSetting.PropertyChanged += _proxiedPropertyChanged;
    }

    /// <summary>
    /// Handles the PropertyChanged event of the proxied setting and updates the state of the current setting accordingly.
    /// </summary>
    /// <param name="sender">The source of the event, typically the proxied setting.</param>
    /// <param name="args">The event data containing details about the property that changed.</param>
    private void OnProxiedSettingPropertyChanged(object? sender, PropertyChangedEventArgs args) {
        switch (args.PropertyName) {
            case nameof(ProxiedSetting.Value):
                SetValue(GetProxied());
                break;
            case nameof(ProxiedSetting.IsDirty):
                OnPropertyChanged(nameof(IsDirty));
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
            case nameof(ProxiedSetting.ValidationResults):
                OnPropertyChanged(nameof(ValidationResults));
                break;
        }
    }

    /// <inheritdoc />
    public override void Dispose() {
        ProxiedSetting.PropertyChanged -= _proxiedPropertyChanged;
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
        get => IsDirty ? base.Value : GetProxied();
        set {
            if (!ProxiedSetting.HasValue)
                throw new InvalidOperationException($"Underlying {ProxiedSetting.GetType().Name} should be read from device before assigning value");

            base.Value = value;
            // does not allow to change proxied value until is valid
            if (ValidationResults.Any()) return;

            var newProxied = CreateNewProxiedValue(ProxiedSetting.Value ?? throw new InvalidOperationException(),
                value ?? throw new ArgumentNullException(nameof(value)));
            if (ReferenceEquals(newProxied, ProxiedSetting.Value))
                throw new InvalidOperationException($"{nameof(CreateNewProxiedValue)} should not return the same object");
            ProxiedSetting.Value = newProxied;

            if (ProxiedSetting.ValidationResults.Any()) {
                AddValidationResults(ProxiedSetting.ValidationResults);
            }
        }
    }

    /// <inheritdoc />
    public override bool HasValue => ProxiedSetting.HasValue;

    /// <inheritdoc />
    public override bool IsDirty {
        get => ProxiedSetting.IsDirty || base.IsDirty;
        protected set => base.IsDirty = value;
    }

    /// <inheritdoc />
    public override bool IsSynchronized => ProxiedSetting.IsSynchronized;

    /// <inheritdoc />
    public override bool IsError => ProxiedSetting.IsError;

    /// <inheritdoc />
    public override Exception? Exception => ProxiedSetting.Exception;
    /// <summary>
    /// Converts <see cref="ProxiedSetting"/> value to <see cref="Value"/>
    /// </summary>
    /// <returns>Result of conversion</returns>
    protected abstract T? GetProxied();

    /// <summary>
    /// Create new <see cref="ProxiedSetting"/>.<see cref="IDeviceSetting{T}.Value"/>
    /// based on old <paramref name="proxied"/> value with new <paramref name="value"/>.
    /// </summary>
    /// <param name="proxied">Current proxied value</param>
    /// <param name="value"><see cref="IDeviceSetting{T}.Value"/></param>
    /// <returns>new <see cref="ProxiedSetting"/>.<see cref="IDeviceSetting{T}.Value"/></returns>
    protected abstract TProxied CreateNewProxiedValue(TProxied proxied, T value);

    /// <inheritdoc />
    public override Task Reset(CancellationToken cancellationToken) {
        var res = ProxiedSetting.Reset(cancellationToken);
        IsDirty = ProxiedSetting.IsDirty;
        return res;
    }

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (!ValidationResults.Any()) {
            await ProxiedSetting.CommitChanges(cancellationToken);
            IsDirty = ProxiedSetting.IsDirty;
        }
    }

    /// <inheritdoc />
    public override async Task Read(CancellationToken cancellationToken) {
        await ProxiedSetting.Read(cancellationToken);
        IsDirty = ProxiedSetting.IsDirty;
    }
}