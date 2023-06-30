using Polimaster.Device.Abstract.Device.Commands.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings.Interfaces;

public interface IDeviceSettingBuilder<TData> {
    IDeviceSettingBuilder<TData> WithWriteCommand<TValue>(ICommand<TValue, TData> command);
    IDeviceSettingBuilder<TData> WithReadCommand<TValue>(ICommand<TValue, TData> command);

    IDeviceSettingBuilder<TData> WithImplementation<T, TValue>() where T : class, IDeviceSetting<TValue>, new();
    
    IDeviceSetting<TValue> Build<TValue>();

    IDeviceSetting<TIn> BuildWithProxy<T, TIn, TValue>()
        where T : class, IDeviceSettingProxy<TIn, TValue>, new();
}