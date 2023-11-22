using System;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Settings;

/// <inheritdoc cref="ISettingBuilder"/>
public class SettingBuilder : ISettingBuilder {
    private readonly ITransport _transport;
    private object? _reader;
    private object? _writer;
    private Type? _implementation;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="transport">Device transport</param>
    public SettingBuilder(ITransport transport) {
        _transport = transport;
    }

    /// <inheritdoc />
    public ISettingBuilder WithWriter<T>(IDataWriter<T> command) {
        _writer = command;
        return this;
    }

    /// <inheritdoc />
    public ISettingBuilder WithReader<T>(IDataReader<T> command) {
        _reader = command;
        return this;
    }

    /// <inheritdoc />
    public ISettingBuilder WithImplementation<T, TSetting>() where T : IDeviceSetting<TSetting> {
        _implementation = typeof(T);
        return this;
    }

    /// <inheritdoc />
    public IDeviceSetting<TValue> Build<TValue>() {
        if(_reader == null) throw new NullReferenceException($"Cant build without reader, assign it with {nameof(WithReader)}()");
        if (_reader is not IDataReader<TValue> reader) throw new ArgumentException($"{_reader.GetType()} cant be assigned as reader for {typeof(TValue)}");
        IDataWriter<TValue>? writer = null;
        if (_writer != null) {
            if(_writer is not IDataWriter<TValue> c) throw new ArgumentException($"{_reader.GetType()} cant be assigned as writer for {typeof(TValue)}");
            writer = c;
        }
        
        var impl = _implementation == null
            ? new DeviceSettingBase<TValue>(_transport, reader, writer)
            : (IDeviceSetting<TValue>)Activator.CreateInstance(_implementation, _transport, reader, writer);

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
        _reader = null;
        _writer = null;
        _implementation = null;
    }
}