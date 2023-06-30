using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;

public interface IDeviceSettingProxy<TIn, TValue> : IDeviceSetting<TIn> {
    IDeviceSetting<TValue>? ProxiedSetting { get; set; }
    TIn FromProxied(TValue? value);
    TValue FromCommand(TIn? value);
}

public abstract class ADeviceSettingProxy<TIn, TValue> : ADeviceSettings<TIn>, IDeviceSettingProxy<TIn, TValue> {
    public IDeviceSetting<TValue>? ProxiedSetting { get; set; }

    public override TIn? Value {
        get => ProxiedSetting != null ? FromProxied(ProxiedSetting.Value) : default;
        set {
            if (ProxiedSetting != null) ProxiedSetting.Value = FromCommand(value);
        }
    }

    public abstract TIn FromProxied(TValue? value);
    public abstract TValue FromCommand(TIn? value);

    public override async Task CommitChanges(CancellationToken cancellationToken) {
        if (ProxiedSetting != null) await ProxiedSetting.CommitChanges(cancellationToken);
    }

    public override async Task Read(CancellationToken cancellationToken) {
        if (ProxiedSetting != null) await ProxiedSetting.Read(cancellationToken);
    }
}