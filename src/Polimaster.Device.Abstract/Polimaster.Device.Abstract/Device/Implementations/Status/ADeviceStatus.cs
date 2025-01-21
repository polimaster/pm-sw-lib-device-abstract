using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport;

namespace Polimaster.Device.Abstract.Device.Implementations.Status;

/// <summary>See <see cref="IDeviceStatus{TStatus}"/></summary>
/// <typeparam name="TStatus">Data type for <see cref="IDeviceStatus{TStatus}"/></typeparam>
/// <typeparam name="TStream"></typeparam>
public abstract class ADeviceStatus<TStatus, TStream> : IDeviceStatus<TStatus> {
    /// <summary>
    /// See <see cref="ITransport{TStream}"/>
    /// </summary>
    protected ITransport<TStream> Transport { get; }

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
    protected ADeviceStatus(ITransport<TStream> transport, ILoggerFactory? loggerFactory) {
        Transport = transport;
        Logger = loggerFactory?.CreateLogger(GetType());

        Transport.Closing += Stop;
    }
}