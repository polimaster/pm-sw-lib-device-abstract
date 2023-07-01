using System;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

public class DeviceSettingBuilder<TData> : IDeviceSettingBuilder<TData> {
    private object? _readCommand;
    private object? _writeCommand;
    private object? _implementation;

    public IDeviceSettingBuilder<TData> WithWriteCommand<TValue>(ICommand<TValue, TData> command) {
        _writeCommand = command;
        return this;
    }

    public IDeviceSettingBuilder<TData> WithReadCommand<TValue>(ICommand<TValue, TData> command) {
        _readCommand = command;
        return this;
    }

    public IDeviceSettingBuilder<TData> WithImplementation<T, TValue>()
        where T : class, IDeviceSetting<TValue>, new() {
        _implementation = Activator.CreateInstance<T>();
        return this;
    }

    public IDeviceSetting<TValue> Build<TValue>() {
        var impl = _implementation as IDeviceSetting<TValue> ?? new DeviceSettingBase<TValue>();

        var readCommand = _readCommand as ICommand<TValue, TData>;
        var writeCommand = _writeCommand as ICommand<TValue, TData>;

        impl.ReadCommand = readCommand;
        impl.WriteCommand = writeCommand;

        CleanUp();
        return impl;
    }

    public IDeviceSetting<TValue> BuildWithProxy<T, TValue, TProxied>()
        where T : class, IDeviceSettingProxy<TValue, TProxied>, new() {
        var proxy = Activator.CreateInstance<T>();
        proxy.ProxiedSetting = Build<TProxied>();
        return proxy;
    }

    private void CleanUp() {
        _readCommand = null;
        _writeCommand = null;
        _implementation = null;
    }
}