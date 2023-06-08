using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Commands;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device; 

/// <inheritdoc cref="IDevice{TData, TConnectionParams}"/>
public abstract class ADevice<TData, TConnectionParams> : IDevice<TData, TConnectionParams> {
    private readonly ILogger? _logger;

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.DeviceInfo"/>
    public virtual IDeviceInfo? DeviceInfo => null;
    

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.ConnectionParams"/>
    public TConnectionParams? ConnectionParams { get; }

    
    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.ConnectionState"/>
    public virtual ConnectionState ConnectionState { get; }


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
    protected ADevice(ITransport<TData, TConnectionParams> transport, ILogger? logger = null, TConnectionParams? connectionParams = default) {
        _logger = logger;
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
            _logger.Log(LogLevel.Debug, "Writing command {C}", nameof(command.GetType));
            var stream = await Transport.Open(ConnectionParams);
            await Transport.Write(stream, command.Compile(), cancellationToken);
        } catch (Exception e) {
            throw new DeviceException(e);
        }
    }

    /// <inheritdoc cref="IDevice{TData,TConnectionParams}.Read{TResult,TParam}"/>
    public virtual async Task<TResult?> Read<TResult, TParam>(IReadCommand<TResult, TParam, TData> command, CancellationToken cancellationToken = new()) {
        try {
            _logger.Log(LogLevel.Debug, "Reading command {C}", nameof(command.GetType));
            var stream = await Transport.Open(ConnectionParams);
            var res = await Transport.Read(stream, command.Compile(), cancellationToken);
            return command.Parse(res);
        } catch (Exception e) {
            throw new DeviceException(e);
        }
    }

    public void Dispose() {
        Transport.Dispose();
    }
}