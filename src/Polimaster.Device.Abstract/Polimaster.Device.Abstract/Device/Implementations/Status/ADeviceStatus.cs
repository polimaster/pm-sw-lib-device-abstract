using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.Status;

/// <summary>See <see cref="IDeviceStatus{TStatus}"/></summary>
/// <typeparam name="TStatus">Data type for <see cref="IDeviceStatus{TStatus}"/></typeparam>
/// <typeparam name="T">Data type for <see cref="ITransport{T}"/></typeparam>
public abstract class ADeviceStatus<T, TStatus> : IDeviceStatus<TStatus> {
    /// <summary>
    /// <see cref="ITransport{T}"/>
    /// </summary>
    protected ITransport<T> Transport { get; }

    /// <summary>
    /// Logger
    /// </summary>
    protected readonly ILogger? Logger;

    /// <inheritdoc />
    public abstract Task<TStatus> Read(CancellationToken token);

    /// <inheritdoc />
    public abstract void Start(CancellationToken token);

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public abstract event Action<TStatus> HasNext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="transport"></param>
    /// <param name="loggerFactory"></param>
    protected ADeviceStatus(ITransport<T> transport, ILoggerFactory? loggerFactory) {
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());

        Transport.Closing += Stop;
    }
}