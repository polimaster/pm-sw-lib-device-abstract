using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device;

/// <inheritdoc cref="IDeviceBuilder{TTransport}"/>
public class DeviceBuilder<TTransport> : IDeviceBuilder<TTransport> {
    private readonly ILoggerFactory? _loggerFactory;
    private readonly ICommandBuilder<TTransport>? _commandBuilder;
    private readonly IDeviceSettingBuilder<TTransport>? _settingsBuilder;
    private ITransport<TTransport>? _transport;

    public DeviceBuilder(ILoggerFactory? loggerFactory, ICommandBuilder<TTransport>? commandBuilder,
        IDeviceSettingBuilder<TTransport>? settingsBuilder) {
        _loggerFactory = loggerFactory;
        _commandBuilder = commandBuilder;
        _settingsBuilder = settingsBuilder;
    }

    public IDeviceBuilder<TTransport> With(ITransport<TTransport> transport) {
        _transport = transport;
        return this;
    }

    public T Build<T>() where T : class, IDevice<TTransport>, new() {
        if (_commandBuilder == null) throw new ArgumentException("Cant build device without command builder");
        if (_settingsBuilder == null) throw new ArgumentException("Cant build device without settings builder");
        if (_transport == null) throw new ArgumentException("Cant build device without transport layer");

        var device = new T {
            Transport = _transport,
            CommandBuilder = _commandBuilder,
            SettingBuilder = _settingsBuilder,
            Logger = _loggerFactory?.CreateLogger<IDevice<TTransport>>()
        };
        device.BuildSettings();

        CleanUp();

        return device;
    }

    private void CleanUp() {
        _transport = null;
    }
}