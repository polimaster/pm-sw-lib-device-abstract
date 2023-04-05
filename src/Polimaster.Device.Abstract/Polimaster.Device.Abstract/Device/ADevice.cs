using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device; 

/// <inheritdoc cref="IDevice{TData}"/>
public abstract class ADevice<TData> : IDevice<TData> {
    
    /// <inheritdoc cref="IDevice{TData}.Transport"/>
    public ITransport<TData> Transport { get; }

    /// <summary>
    /// Device constructor
    /// </summary>
    /// <param name="transport">
    /// <see cref="Transport"/>
    /// </param>
    public ADevice(ITransport<TData> transport) {
        Transport = transport;
    }

    /// <inheritdoc cref="IDevice{TData}.Write{TParam}"/>
    public virtual void Write<TParam>(ref ICommand<TParam, TData> command) {
        Transport.Write(command.Compile());
    }

    /// <inheritdoc cref="IDevice{TData}.Read{TResult,TParam}"/>
    public virtual void Read<TResult, TParam>(ref IReadCommand<TResult, TParam, TData> command) {
        var res = Transport.Read(command.Compile());
        command.Parse(res);
    }
}