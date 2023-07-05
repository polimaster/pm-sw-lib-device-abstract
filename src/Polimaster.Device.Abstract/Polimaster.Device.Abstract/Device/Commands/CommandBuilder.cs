using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands;


/// <inheritdoc cref="ICommandBuilder"/>
public class CommandBuilder : ICommandBuilder {
    
    private readonly ILoggerFactory? _loggerFactory;
    public CommandBuilder(ILoggerFactory? loggerFactory = null) {
        _loggerFactory = loggerFactory;
    }
    
    public ICommandBuilder<TTransport> Create<TTransport>(IDevice<TTransport> device) {
        return new CommandBuilder<TTransport>(_loggerFactory) {
            Device = device
        };
    }
}

public class CommandBuilder<TTransport> : ICommandBuilder<TTransport> {
    private readonly ILoggerFactory? _loggerFactory;
    private ILogger? _logger;

    public CommandBuilder(ILoggerFactory? loggerFactory = null) {
        _loggerFactory = loggerFactory;
    }

    public IDevice<TTransport>? Device { get; set; }

    public ICommandBuilder<TTransport> With(ILogger? logger) {
        _logger = logger;
        return this;
    }

    public T Build<T, TCommand>(TCommand? initialData = default) where T : class, ICommand<TCommand, TTransport>, new() {
        if (Device == null) throw new NullReferenceException($"{nameof(Device)} parameter for command is null");

        var result = new T {
            Device = Device,
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