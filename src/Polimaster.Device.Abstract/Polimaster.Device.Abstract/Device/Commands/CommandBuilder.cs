using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands;


public class CommandBuilder<TTransport> : ICommandBuilder<TTransport> {
    private ILoggerFactory? _loggerFactory;
    private ILogger? _logger;

    private readonly Dictionary<string, object> _commands = new();

    public ICommandBuilder<TTransport> With(ILoggerFactory? factory) {
        _loggerFactory = factory;
        return this;
    }

    public ICommandBuilder<TTransport> With(ILogger? logger) {
        _logger = logger;
        return this;
    }

    public ICommand<TCommand, TTransport> Build<T, TCommand>(IDevice<TTransport> device)
        where T : class, ICommand<TCommand, TTransport>, new() {
        var key = GetKey<T>(device);
        var found = _commands.FirstOrDefault(e => e.Key == key);
        if (found.Key != null) return (ICommand<TCommand, TTransport>)found.Value;

        var result = new T {
            Transport = device.Transport,
            Logger = _loggerFactory?.CreateLogger<T>() ?? _logger
        };

        _commands.Add(key, result);
        device.IsDisposing += () => CleanUpDevice(device);

        CleanUp();

        return result;
    }

    private void CleanUp() {
        _loggerFactory = null;
        _logger = null;
    }

    private void CleanUpDevice(IDevice device) {
        var found = _commands.Where(e => e.Key.StartsWith($"{device.Id}>"));
        foreach (var pair in found) _commands.Remove(pair.Key);
    }

    private static string GetKey<T>(IDevice device) {
        return $"{device.Id}>{nameof(T)}";
    }
}