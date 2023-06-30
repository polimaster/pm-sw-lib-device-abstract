using System;
using Polimaster.Device.Abstract.Commands;

namespace Polimaster.Device.Abstract.Device.Settings;

public class DeviceSettingBuilder : IDeviceSettingBuilder {
    private object? _readCommand;
    private object? _writeCommand;
    private object? _implementation;
    private object? _proxy;

    public IDeviceSettingBuilder WithWriteCommand<TValue, TData>(ICommand<TValue, TData> command) {
        _writeCommand = command;
        return this;
    }

    public IDeviceSettingBuilder WithReadCommand<TValue, TData>(ICommand<TValue, TData> command) {
        _readCommand = command;
        return this;
    }

    public IDeviceSettingBuilder WithImplementation<T, TValue, TData>()
        where T : class, IDeviceSetting<TValue, TData>, new() {
        _implementation = Activator.CreateInstance<T>();
        return this;
    }

    public IDeviceSettingBuilder WithProxy<T, TIn, TOut, TData>()
        where T : class, IDeviceSettingProxy<TIn, TOut, TData>, new() {
        _proxy = Activator.CreateInstance<T>();
        return this;
    }

    public IDeviceSetting<TValue, TData> Build<TValue, TData>() {
        var impl = _implementation as IDeviceSetting<TValue, TData> ?? new DeviceSettingBase<TValue, TData>();

        var readCommand = _readCommand as ICommand<TValue, TData>;
        var writeCommand = _writeCommand as ICommand<TValue, TData>;

        impl.ReadCommand = readCommand;
        impl.WriteCommand = writeCommand;

        CleanUp();
        return impl;
    }

    public IDeviceSettingProxy<TIn, TValue, TData> Build<TIn, TValue, TData>() {
        switch (_proxy) {
            case null:
                throw new NullReferenceException($"Initiate Setting Proxy with {nameof(WithProxy)} method.");
            case IDeviceSettingProxy<TIn, TValue, TData> p:
                p.ProxiedSetting = Build<TValue, TData>();
                CleanUp();
                return p;
            default:
                throw new Exception();
        }
    }

    private void CleanUp() {
        _readCommand = null;
        _writeCommand = null;
        _implementation = null;
        _proxy = null;
    }
}