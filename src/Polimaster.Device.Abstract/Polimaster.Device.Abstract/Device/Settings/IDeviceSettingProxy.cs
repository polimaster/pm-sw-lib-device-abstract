namespace Polimaster.Device.Abstract.Device.Settings;

public interface IDeviceSettingProxy<TIn, TValue, TData> : IDeviceSetting<TValue, TData> {
    IDeviceSetting<TValue, TData> ProxiedSetting { get; set; }
    TIn FromSetting(TValue value);
    TValue FromCommand(TIn value);
}