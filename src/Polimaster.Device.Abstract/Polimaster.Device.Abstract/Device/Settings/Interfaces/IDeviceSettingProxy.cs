using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;

public interface IDeviceSettingProxy<T, TProxied> : IDeviceSetting<T> {
    IDeviceSetting<TProxied>? ProxiedSetting { get; set; }
    T FromProxied(TProxied? value);
    TProxied FromCommand(T? value);
}

public abstract class ADeviceSettingProxy<T, TProxied> : ADeviceSettings<T>, IDeviceSettingProxy<T, TProxied> {
    public IDeviceSetting<TProxied>? ProxiedSetting { get; set; }

    public override T? Value {
        get => ProxiedSetting != null ? FromProxied(ProxiedSetting.Value) : default;
        set {
            if (ProxiedSetting != null) ProxiedSetting.Value = FromCommand(value);
        }
    }

    public abstract T FromProxied(TProxied? value);
    public abstract TProxied FromCommand(T? value);

    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (ProxiedSetting != null) await ProxiedSetting.CommitChanges(cancellationToken);
    }

    public override async Task Read(CancellationToken cancellationToken) {
        if (ProxiedSetting != null) {
            if (ProxiedSetting.Value != null) return;
            await ProxiedSetting.Read(cancellationToken);
        }
    }
}