using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Exceptions;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc />
public abstract class ACommand<TValue, TCommand> : ICommand<TValue> {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ACommand(ILoggerFactory? loggerFactory = null) {
        Logger = loggerFactory?.CreateLogger(GetType());
    }

    /// <summary>
    /// Command logger
    /// </summary>
    protected ILogger? Logger { get; }

    

    /// <inheritdoc />
    public TValue? Value { get; set; }

    /// <inheritdoc />
    public Action<TValue?>? ValueChanged { get; set; }

    /// <summary>
    /// Returns formatted command to be send to device
    /// </summary>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TCommand Compile();

    /// <summary>
    /// Validates command or/and its parameters before execution.
    /// </summary>
    /// <exception cref="CommandValidationException">Throws if validation failed.</exception>
    protected virtual void Validate(){}

    /// <summary>
    /// Log current command
    /// </summary>
    /// <param name="methodName"></param>
    protected void LogCommand(string methodName) => Logger?.LogDebug("Call {N} with command {C}", methodName, GetType().Name);

    /// <summary>
    /// Log error
    /// </summary>
    /// <param name="e"></param>
    /// <param name="methodName"></param>
    protected void LogError(Exception e, string methodName) => Logger?.LogError(e, "Error while sending {N} command {C}",methodName, GetType().Name);

    /// <inheritdoc />
    public abstract Task Send(IDeviceStream stream, ushort sleep, CancellationToken cancellationToken = new());
}