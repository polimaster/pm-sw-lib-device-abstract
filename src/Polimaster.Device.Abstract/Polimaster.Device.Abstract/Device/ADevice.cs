using System;
using System.Threading.Tasks;
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
    public virtual async Task Write<TParam>(ICommand<TParam, TData> command) {
        try {
            await Transport.Open();
            await Transport.Write(command.Compile());
        } catch (Exception e) {
            throw new DeviceException(e);
        }
    }

    /// <inheritdoc cref="IDevice{TData}.Read{TResult,TParam}"/>
    public virtual async Task<TResult?> Read<TResult, TParam>(IReadCommand<TResult, TParam, TData> command) {
        try {
            await Transport.Open();
            var res = await Transport.Read(command.Compile());
            return command.Parse(res);
        } catch (Exception e) {
            throw new DeviceException(e);
        }
    }

    public void Dispose() {
        Transport.Dispose();
    }
}