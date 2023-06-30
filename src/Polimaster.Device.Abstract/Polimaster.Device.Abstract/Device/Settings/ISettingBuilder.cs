using System;
using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings; 

public interface IDeviceSettingBuilder {
    IDeviceSettingBuilder WithWriteCommand<TValue, TData>(ICommand<TValue, TData> command);
    IDeviceSettingBuilder WithReadCommand<TValue, TData>(ICommand<TValue, TData> command);
    IDeviceSettingBuilder WithImplementation<T, TValue, TData>() where T : class, IDeviceSetting<TValue, TData>, new();
    IDeviceSetting<TValue, TData> Build<TValue, TData>();
}

public class DeviceSettingBuilder  : IDeviceSettingBuilder {
    private object? _readCommand;
    private object? _writeCommand;
    private object? _implementation;

    public IDeviceSettingBuilder WithWriteCommand<TValue, TData>(ICommand<TValue, TData> command) {
        _writeCommand = command;
        return this;
    }

    public IDeviceSettingBuilder WithReadCommand<TValue, TData>(ICommand<TValue, TData> command) {
        _readCommand = command;
        return this;
    }

    public IDeviceSettingBuilder WithImplementation<T, TValue, TData>() where T : class, IDeviceSetting<TValue, TData>, new() {
        _implementation = Activator.CreateInstance<T>();
        return this;
    }

    public IDeviceSetting<TValue, TData> Build<TValue, TData>() {
        var impl = _implementation as IDeviceSetting<TValue, TData> ?? new DeviceSettingBase<TValue, TData>();

        var readCommand = _readCommand as ICommand<TValue, TData>;
        var writeCommand = _writeCommand as ICommand<TValue, TData>;

        impl.ReadCommand = readCommand;
        impl.WriteCommand = writeCommand;

        return impl;
    }
}