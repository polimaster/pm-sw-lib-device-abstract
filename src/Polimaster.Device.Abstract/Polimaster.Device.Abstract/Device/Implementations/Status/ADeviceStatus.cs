using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Device.Implementations.Status;

/// <inheritdoc />
public abstract class ADeviceStatus<T> : IDeviceStatus<T> {
    /// <summary>
    /// Logger factory
    /// </summary>
    protected ILoggerFactory? LoggerFactory { get; }
    /// <summary>
    /// Logger
    /// </summary>
    protected ILogger? Logger { get; }

    /// <inheritdoc />
    public abstract Task<T> Read(CancellationToken token = new());

    /// <inheritdoc />
    public abstract void Start(CancellationToken token = new());

    /// <inheritdoc />
    public abstract void Stop();

    /// <inheritdoc />
    public abstract event Action<T> HasNext;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected ADeviceStatus(ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Logger = loggerFactory?.CreateLogger(GetType());
    }
}