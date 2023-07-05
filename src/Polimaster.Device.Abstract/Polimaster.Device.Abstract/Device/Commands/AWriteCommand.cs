using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Device.Commands.Interfaces;
using Polimaster.Device.Abstract.Device.Interfaces;
using Polimaster.Device.Abstract.Transport.Interfaces;

namespace Polimaster.Device.Abstract.Device.Commands;

/// <summary>
/// Command with no result returned while <see cref="ITransport{TData}.Write"/>
/// </summary>
/// <inheritdoc cref="ICommand{T,TTransport}"/>
public abstract class AWriteCommand<T, TTransport> : ICommand<T, TTransport> {
    public Action<T?>? ValueChanged { get; set; }
    // public ITransport<TTransport>? Transport { get; set; }
    public IDevice<TTransport> Device { get; set; } = null!;

    public ICommandBuilder<TTransport> CommandBuilder { get; set; } = null!;
    public ILogger? Logger { get; set; }
    
    public T? Value { get; set; }

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

    protected async Task<Stream> Prepare() {
        if (Device.Transport == null) throw new NullReferenceException(
            $"Transport for {GetType().Name} is null. Consider using {nameof(ICommandBuilder<TTransport>)} while creating commands.");
        Validate();
        var stream = await Device.Transport.Open();
        if (stream == null) throw new NullReferenceException("Transport stream is null");
        Logger?.LogDebug("Executing command {C}", GetType().Name);
        return stream;
    }
    
    public virtual async Task Send(CancellationToken cancellationToken = new()) {
        if (cancellationToken.IsCancellationRequested) return;
        var stream = await Prepare();
        if (cancellationToken.IsCancellationRequested) return;
        await Device.Transport.Write(stream, Compile(), cancellationToken);
        ValueChanged?.Invoke(Value);
    }
}