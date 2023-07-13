using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands;

public class CommandBuilder : ICommandBuilder {
    private readonly ILoggerFactory? _loggerFactory;
    private ILogger? _logger;
    private IDevice? _device;

    public CommandBuilder(ILoggerFactory? loggerFactory = null) {
        _loggerFactory = loggerFactory;
    }

    public ICommandBuilder With(ILogger? logger) {
        _logger = logger;
        return this;
    }

    public ICommandBuilder With(IDevice? device) {
        _device = device;
        return this;
    }

    public T Build<T, TCommand>(TCommand? initialData = default) where T : class, ICommand<TCommand>, new() {
        if (_device == null) throw new NullReferenceException("Device for command is null");

        var result = new T {
            Device = _device,
            Logger = _logger ?? _loggerFactory?.CreateLogger<T>(),
            Value = initialData
        };

        CleanUp();

        return result;
    }

    private void CleanUp() {
        _logger = null;
    }
}