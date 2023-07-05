using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device;

/// <inheritdoc cref="IDeviceBuilder{TTransport}"/>
public class DeviceBuilder<TTransport> : IDeviceBuilder<TTransport> {
    private readonly ILoggerFactory? _loggerFactory;
    private ITransport<TTransport>? _transport;
    private readonly ICommandBuilder _commandBuilder;
    private readonly IDeviceSettingBuilder<TTransport> _settingsBuilder;

    public DeviceBuilder(ICommandBuilder commandBuilder,
        IDeviceSettingBuilder<TTransport> settingsBuilder, ILoggerFactory? loggerFactory = null) {
        _commandBuilder = commandBuilder;
        _settingsBuilder = settingsBuilder;
        _loggerFactory = loggerFactory;
    }

    public IDeviceBuilder<TTransport> With(ITransport<TTransport> transport) {
        _transport = transport;
        return this;
    }

    public T Build<T>() where T : class, IDevice<TTransport>, new() {
        if (_transport == null) throw new DeviceException("Cant build device without transport layer");

        var device = new T {
            Transport = _transport,
            SettingBuilder = _settingsBuilder,
            Logger = _loggerFactory?.CreateLogger<IDevice<TTransport>>()
        };
        device.CommandBuilder = _commandBuilder.Create(device);
        
        device.BuildSettings();

        CleanUp();

        return device;
    }

    private void CleanUp() {
        _transport = null;
    }
}