using System;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc cref="IDeviceSettingBuilder"/>
public class DeviceSettingBuilder : IDeviceSettingBuilder {
    private object? _readCommand;
    private object? _writeCommand;
    private object? _implementation;

    public IDeviceSettingBuilder WithWriteCommand<TValue, TTransport>(ICommand<TValue, TTransport> command) {
        _writeCommand = command;
        return this;
    }

    public IDeviceSettingBuilder WithReadCommand<TValue, TTransport>(ICommand<TValue, TTransport> command) {
        _readCommand = command;
        return this;
    }

    public IDeviceSettingBuilder WithImplementation<T, TValue>()
        where T : class, IDeviceSetting<TValue>, new() {
        _implementation = Activator.CreateInstance<T>();
        return this;
    }

    public IDeviceSetting<TValue> Build<TValue>() {
        var impl = _implementation as IDeviceSetting<TValue> ?? new DeviceSettingBase<TValue>();

        var readCommand = _readCommand as ICommand<TValue>;
        var writeCommand = _writeCommand as ICommand<TValue>;

        impl.ReadCommand = readCommand;
        impl.WriteCommand = writeCommand;

        CleanUp();
        return impl;
    }

    public T BuildWithProxy<T, TValue, TProxied>()
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