using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device;

namespace Polimaster.Device.Abstract.Commands;

public interface ICommandBuilder {
    ICommandBuilder With(ILoggerFactory? factory);
    ICommandBuilder With(ILogger? logger);

    ICommand<TValue, TData> Build<T, TValue, TData>(IDevice<TData> device)
        where T : class, ICommand<TValue, TData>, new();
}

public class CommandBuilder : ICommandBuilder {
    private ILoggerFactory? _loggerFactory;
    private ILogger? _logger;

    private static readonly Dictionary<string, object> COMMANDS = new();

    public ICommandBuilder With(ILoggerFactory? factory) {
        _loggerFactory = factory;
        return this;
    }

    public ICommandBuilder With(ILogger? logger) {
        _logger = logger;
        return this;
    }

    public ICommand<TValue, TData> Build<T, TValue, TData>(IDevice<TData> device)
        where T : class, ICommand<TValue, TData>, new() {
        var key = GetKey<T>(device);
        var found = COMMANDS.FirstOrDefault(e => e.Key == key);
        if (found.Key != null) return (ICommand<TValue, TData>)found.Value;

        var result = new T {
            Transport = device.Transport,
            Logger = _loggerFactory?.CreateLogger<T>() ?? _logger
        };

        COMMANDS.Add(key, result);
        device.IsDisposing += () => CleanUpDevice(device);

        CleanUp();

        return result;
    }

    private void CleanUp() {
        _loggerFactory = null;
        _logger = null;
    }

    private static void CleanUpDevice(IDevice device) {
        var found = COMMANDS.Where(e => e.Key.StartsWith($"{device.Id}>"));
        foreach (var pair in found) COMMANDS.Remove(pair.Key);
    }

    private static string GetKey<T>(IDevice device) {
        return $"{device.Id}>{nameof(T)}";
    }
}