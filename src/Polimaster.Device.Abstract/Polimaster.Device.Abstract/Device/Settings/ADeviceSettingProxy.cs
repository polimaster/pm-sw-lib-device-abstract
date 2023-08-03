using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;


/// <inheritdoc cref="IDeviceSettingProxy{T,TProxied}"/>
public abstract class ADeviceSettingProxy<T, TProxied> : ADeviceSetting<T>, IDeviceSettingProxy<T, TProxied> {
    /// <inheritdoc />
    public IDeviceSetting<TProxied>? ProxiedSetting { get; set; }

    /// <inheritdoc />
    public override T? Value {
        get => ProxiedSetting != null ? FromProxied(ProxiedSetting.Value) : default;
        set {
            if (ProxiedSetting != null) ProxiedSetting.Value = ToProxied(value);
        }
    }

    /// <inheritdoc />
    public abstract T? FromProxied(TProxied? value);

    /// <inheritdoc />
    public abstract TProxied? ToProxied(T? value);

    /// <inheritdoc />
    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (ProxiedSetting != null) await ProxiedSetting.CommitChanges(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task Read(CancellationToken cancellationToken) {
        if (ProxiedSetting != null) {
            if (ProxiedSetting.Value != null) return;
            await ProxiedSetting.Read(cancellationToken);
        }
    }
}