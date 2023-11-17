using System;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc cref="ISettingBuilder"/>
public class SettingBuilder : ISettingBuilder {
    private readonly TTransport _transport;
    private object? _readCommand;
    private object? _writeCommand;
    private Type? _implementation;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="transport">Device transport</param>
    public SettingBuilder(TTransport transport) {
        _transport = transport;
    }

    /// <inheritdoc />
    public ISettingBuilder WithWriteCommand<TValue, T>(ICommand<TValue, T> command) {
        _writeCommand = command;
        return this;
    }

    /// <inheritdoc />
    public ISettingBuilder WithReadCommand<TValue, T>(ICommand<TValue, T> command) {
        _readCommand = command;
        return this;
    }

    /// <inheritdoc />
    public ISettingBuilder WithImplementation<T, TSetting>() where T : IDeviceSetting<TSetting> {
        _implementation = typeof(T);
        return this;
    }

    /// <inheritdoc />
    public IDeviceSetting<TValue> Build<TValue>() {
        if (_readCommand is not ICommand<TValue, T> readCommand) throw new NullReferenceException("Read command cant be null");
        var writeCommand = _writeCommand as ICommand<TValue, T>;

        var impl = _implementation == null
            ? new DeviceSettingBase<TValue>(_transport, readCommand, writeCommand)
            : (IDeviceSetting<TValue>)Activator.CreateInstance(_implementation, _transport, readCommand, writeCommand);

        CleanUp();
        return impl;
    }

    /// <inheritdoc />
    public T BuildWithProxy<T, TValue, TProxied>()
        where T : ADeviceSettingProxy<TValue, TProxied> {
        var proxied = Build<TProxied>();
        var proxy = (T)Activator.CreateInstance(typeof(T), proxied);
        return proxy;
    }

    private void CleanUp() {
        _readCommand = null;
        _writeCommand = null;
        _implementation = null;
    }
}