using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <inheritdoc />
public abstract class ACommand<T, TTransport> : ICommand<T>{
    /// <inheritdoc />
    public Action<T?>? ValueChanged  { get; set; }

    /// <inheritdoc />
    public IDevice Device { get; set; } = null!;

    /// <inheritdoc />
    public T? Value { get; set; }

    /// <inheritdoc />
    public ILogger? Logger { get; set; }
    
    /// <summary>
    /// Returns formatted command to be send to device
    /// </summary>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TTransport Compile();
    
    /// <summary>
    /// Validates command or/and its parameters before execution.
    /// </summary>
    /// <exception cref="CommandValidationException">Throws if validation failed.</exception>
    protected virtual void Validate(){}

    /// <summary>
    /// Parse value returned by <see cref="IDeviceStream"/> while <see cref="Read"/>.
    /// </summary>
    /// <param name="value">Value for parsing</param>
    /// <exception cref="CommandResultParsingException"></exception>
    /// <returns></returns>
    protected abstract T? Parse(TTransport? value);
    
    /// <summary>
    /// Sends command to device. In common it just calls <see cref="Write"/> or <see cref="Read"/> method.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task Send(CancellationToken cancellationToken = new());

    /// <summary>
    /// Writes data to device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task Write(CancellationToken cancellationToken = new());

    /// <summary>
    /// Reads data from device
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected abstract Task Read(CancellationToken cancellationToken = new());

    /// <summary>
    /// Log current command
    /// </summary>
    /// <param name="methodName"></param>
    protected void LogCommand(string methodName) {
        Logger?.LogDebug("Call {N} with command {C}", methodName, GetType().Name);
    }

    /// <summary>
    /// Log error
    /// </summary>
    /// <param name="e"></param>
    /// <param name="methodName"></param>
    protected void LogError(Exception e, string methodName) {
        Logger?.LogError(e, "Error while sending {N} command {C}",methodName, GetType().Name);
    }
}