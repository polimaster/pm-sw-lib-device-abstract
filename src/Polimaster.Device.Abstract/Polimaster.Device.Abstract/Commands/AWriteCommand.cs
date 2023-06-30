using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Commands;

/// <summary>
/// Command with no result returned while <see cref="ITransport{TData}.Write"/>
/// </summary>
/// <typeparam name="TValue"><inheritdoc cref="ICommand{TValue,TTransportData}"/></typeparam>
/// <typeparam name="TTransportData"><inheritdoc cref="ICommand{TValue,TTransportData}"/></typeparam>
public abstract class AWriteCommand<TValue, TTransportData> : ICommand<TValue, TTransportData> {
    public Action<TValue?>? ValueChanged { get; set; }
    public ITransport<TTransportData>? Transport { get; set; }
    public ILogger? Logger { get; set; }
    
    public TValue? Value { get; set; }

    /// <summary>
    /// Returns formatted command to be send to device
    /// </summary>
    /// <exception cref="CommandCompilationException"></exception>
    protected abstract TTransportData Compile();
    
    /// <summary>
    /// Validates command or/and its parameters before execution.
    /// </summary>
    /// <exception cref="CommandValidationException"></exception>
    protected virtual void Validate(){}

    protected async Task<Stream> Prepare() {
        if (Transport == null) throw new NullReferenceException(
            $"Transport for {GetType().Name} is null. Consider using {nameof(ICommandBuilder)} while creating commands.");
        Validate();
        var stream = await Transport!.Open();
        if (stream == null) throw new NullReferenceException("Transport stream is null");
        Logger?.LogDebug("Executing command {C}", GetType().Name);
        return stream;
    }
    
    public virtual async Task Send(CancellationToken cancellationToken = new()) {
        var stream = await Prepare();
        if (cancellationToken.IsCancellationRequested) return;
        await Transport!.Write(stream, Compile(), cancellationToken);
        ValueChanged?.Invoke(Value);
    }
}