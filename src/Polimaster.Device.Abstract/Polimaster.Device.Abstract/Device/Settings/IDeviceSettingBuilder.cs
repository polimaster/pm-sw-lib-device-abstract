using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings; 

public interface IDeviceSettingBuilder {
    IDeviceSettingBuilder WithWriteCommand<TValue, TData>(ICommand<TValue, TData> command);
    IDeviceSettingBuilder WithReadCommand<TValue, TData>(ICommand<TValue, TData> command);
    IDeviceSettingBuilder WithImplementation<T, TValue, TData>() where T : class, IDeviceSetting<TValue, TData>, new();
    IDeviceSettingBuilder WithProxy<T, TIn, TValue, TData>() where T : class, IDeviceSettingProxy<TIn, TValue, TData>, new();
    IDeviceSetting<TValue, TData> Build<TValue, TData>();

    IDeviceSettingProxy<TIn, TValue, TData> Build<TIn, TValue, TData>();
}