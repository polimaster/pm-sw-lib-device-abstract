using Polimaster.Device.Abstract.Device.Commands.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;

public interface IDeviceSettingBuilder<TData> {
    IDeviceSettingBuilder<TData> WithWriteCommand<TValue>(ICommand<TValue, TData> command);
    IDeviceSettingBuilder<TData> WithReadCommand<TValue>(ICommand<TValue, TData> command);

    IDeviceSettingBuilder<TData> WithImplementation<T, TValue>() where T : class, IDeviceSetting<TValue>, new();
    
    IDeviceSetting<TValue> Build<TValue>();

    IDeviceSetting<TValue> BuildWithProxy<T, TValue, TProxied>()
        where T : class, IDeviceSettingProxy<TValue, TProxied>, new();
}