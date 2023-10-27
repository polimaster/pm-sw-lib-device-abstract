using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Device.Settings.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device;

/// <inheritdoc cref="IDeviceBuilder"/>
public class DeviceBuilder : IDeviceBuilder {
    private readonly ILoggerFactory? _loggerFactory;
    private ITransport? _transport;
    // private readonly ICommandBuilder _commandBuilder;
    private readonly ISettingBuilder _settingsBuilder;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="settingsBuilder"></param>
    /// <param name="loggerFactory"></param>
    public DeviceBuilder(ISettingBuilder settingsBuilder, ILoggerFactory? loggerFactory = null) {
        _settingsBuilder = settingsBuilder;
        _loggerFactory = loggerFactory;
    }

    /// <inheritdoc />
    public IDeviceBuilder With(ITransport transport) {
        _transport = transport;
        return this;
    }

    /// <inheritdoc />
    public T Build<T>() where T : class, IDevice, new() {
        if (_transport == null) throw new DeviceException("Cant build device without transport layer");

        var device = new T {
            Transport = _transport,
            SettingBuilder = _settingsBuilder,
            Logger = _loggerFactory?.CreateLogger<IDevice>()
        };
        
        var cb = new CommandBuilder(_loggerFactory);
        device.CommandBuilder = cb.With(device);
        device.BuildSettings();

        CleanUp();

        return device;
    }

    private void CleanUp() {
        _transport = null;
    }
}