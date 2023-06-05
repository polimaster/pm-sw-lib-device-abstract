using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Polimaster.Device.Abstract.Transport;
using Polimaster.Device.Abstract.Transport.Commands;

namespace Polimaster.Device.Abstract.Device; 

/// <inheritdoc cref="IDevice{TData, TConnectionParams}"/>
public abstract class ADevice<TData, TConnectionParams> : IDevice<TData, TConnectionParams> {
    
    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.DeviceInfo"/>
    public virtual IDeviceInfo? DeviceInfo => null;
    

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.ConnectionParams"/>
    public TConnectionParams? ConnectionParams { get; }

    
    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.ConnectionState"/>
    public virtual ConnectionState ConnectionState { get; private set; }


    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.ConnectionStateChanged"/>
    public event Action<ConnectionState>? ConnectionStateChanged;

    
    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Transport"/>
    public ITransport<TData, TConnectionParams> Transport { get; }

    /// <summary>
    /// Device constructor
    /// </summary>
    /// <param name="transport">
    ///     <see cref="Transport"/>
    /// </param>
    /// <param name="connectionParams"></param>
    protected ADevice(ITransport<TData, TConnectionParams> transport, TConnectionParams? connectionParams = default) {
        Transport = transport;
        ConnectionParams = connectionParams;
        // Transport.ConnectionStateChanged += state => {
        //     ConnectionState = state;
        //     ConnectionStateChanged.Invoke(ConnectionState);
        // };
    }

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Write{TParam}"/>
    public virtual async Task Write<TParam>(ICommand<TParam, TData> command, CancellationToken cancellationToken = new()) {
        try {
            await Transport.Open(ConnectionParams);
            await Transport.Write(command.Compile(), cancellationToken);
        } catch (Exception e) {
            throw new DeviceException(e);
        }
    }

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Read{TResult,TParam}"/>
    public virtual async Task<TResult?> Read<TResult, TParam>(IReadCommand<TResult, TParam, TData> command, CancellationToken cancellationToken = new()) {
        try {
            await Transport.Open(ConnectionParams);
            var res = await Transport.Read(command.Compile(), cancellationToken);
            return command.Parse(res);
        } catch (Exception e) {
            throw new DeviceException(e);
        }
    }

    public void Dispose() {
        Transport.Dispose();
    }
}