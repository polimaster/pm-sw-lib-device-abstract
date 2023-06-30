using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands.Interfaces;

public interface ICommandBuilder<TData> {
    ICommandBuilder<TData> With(ILoggerFactory? factory);
    ICommandBuilder<TData> With(ILogger? logger);

    ICommand<TValue, TData> Build<T, TValue>(IDevice<TData> device)
        where T : class, ICommand<TValue, TData>, new();
}

public class CommandBuilder<TData> : ICommandBuilder<TData> {
    private ILoggerFactory? _loggerFactory;
    private ILogger? _logger;

    private readonly Dictionary<string, object> _commands = new();

    public ICommandBuilder<TData> With(ILoggerFactory? factory) {
        _loggerFactory = factory;
        return this;
    }

    public ICommandBuilder<TData> With(ILogger? logger) {
        _logger = logger;
        return this;
    }

    public ICommand<TValue, TData> Build<T, TValue>(IDevice<TData> device)
        where T : class, ICommand<TValue, TData>, new() {
        var key = GetKey<T>(device);
        var found = _commands.FirstOrDefault(e => e.Key == key);
        if (found.Key != null) return (ICommand<TValue, TData>)found.Value;

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