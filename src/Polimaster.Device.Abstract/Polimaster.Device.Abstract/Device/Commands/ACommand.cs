using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands; 

public abstract class ACommand<T, TTransport> : ICommand<T>{
    public Action<T?>? ValueChanged  { get; set; }
    public IDevice Device { get; set; } = null!;
    
    public T? Value { get; set; }
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

    protected abstract T? Parse(TTransport? value);
    
    public abstract Task Send(CancellationToken cancellationToken = new());

    protected abstract Task Write(CancellationToken cancellationToken = new());

    protected abstract Task Read(CancellationToken cancellationToken = new());


    protected void LogCommand(string methodName) {
        Logger?.LogDebug("Call {N} with command {C}", methodName, GetType().Name);
    }

    protected void LogError(Exception e, string methodName) {
        Logger?.LogError(e, "Error while sending {N} command {C}",methodName, GetType().Name);
    }
}