using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;


/// <summary>
/// Device command
/// </summary>
public interface ICommand {
    
    /// <summary>
    /// Send command to device
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns></returns>
    Task Send(CancellationToken cancellationToken);
}

/// <summary>
/// Device command with parameter
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPCommand<T> {
    T? Parameter { get; set; }
}


/// <summary>
/// Device command with result of <see cref="ICommand.Send"/>
/// </summary>
/// <typeparam name="TResult">Type of <see cref="Result"/></typeparam>
public interface ICommand<out TResult> : ICommand {
    
    /// <summary>
    /// Result of <see cref="ICommand.Send"/>
    /// </summary>
    TResult? Result { get; }
}

public interface ITransportCommand<TData> : ICommand {
    ITransport<TData>? Transport { get; set; }
    ILogger? Logger { get; set; }
}

public abstract class ACommand<TData> : ITransportCommand<TData> {
    public ITransport<TData>? Transport { get; set; }
    public ILogger? Logger { get; set; }

    /// <summary>
    /// Returns formatted command to be send to device
    /// </summary>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TData Compile();
    
    /// <summary>
    /// Validates command or/and its parameters before execution.
    /// </summary>
    /// <exception cref="CommandValidationException"></exception>
    protected virtual void Validate(){}
    
    public virtual async Task Send(CancellationToken cancellationToken = new()) {
        CheckTransport();
        Validate();
        var stream = await Transport!.Open();
        if (cancellationToken.IsCancellationRequested) return;
        if (stream == null) throw new NullReferenceException("Transport stream is null");
        Logger?.LogDebug("Executing command {C}", GetType().Name);
        await Transport.Write(stream, Compile(), cancellationToken);
    }

    protected void CheckTransport() {
        if (Transport == null) throw new NullReferenceException(
            $"Transport for {GetType().Name} is null. Consider using {nameof(ICommandFactory<TData>)} while creating commands.");
    }
}

public abstract class AResultCommand<TResult, TData> : ACommand<TData>, ICommand<TResult> {

    public TResult? Result { get; private set; }

    protected abstract TResult Parse(TData data);

    public override async Task Send(CancellationToken cancellationToken = new()) {
        CheckTransport();
        Validate();
        var stream = await Transport!.Open();
        if (cancellationToken.IsCancellationRequested) return;
        if (stream == null) throw new NullReferenceException("Transport stream is null");
        Logger?.LogDebug("Executing command {C}", GetType().Name);
        var res = await Transport.Read(stream, Compile(), cancellationToken);
        Result = Parse(res);
    }
}
