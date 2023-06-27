using System;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

public class CommandFactory<TData> : ICommandFactory<TData> {
    private readonly ITransport<TData> _transport;
    private readonly ILoggerFactory? _loggerFactory;

    public CommandFactory( ITransport<TData> transport, ILoggerFactory? loggerFactory = null) {
        _transport = transport;
        _loggerFactory = loggerFactory;
    }

    public T Create<T>() where T : ITransportCommand<TData>, new() {
        if (_transport == null) throw new NullReferenceException($"{nameof(Transport)} is null");
        
        var t = new T {
            Transport = _transport,
            Logger = _loggerFactory?.CreateLogger<T>()
        };
        return t;
    }
}