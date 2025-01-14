using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polimaster.Device.Abstract.Transport.Stream;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class AClient<TConnectionParams> : IClient where TConnectionParams : IFormattable {
    /// <summary>
    /// Logger factory
    /// </summary>
    protected ILoggerFactory? LoggerFactory { get; }

    /// <summary>
    /// Connection parameters
    /// </summary>
    protected readonly TConnectionParams Params;

    /// <inheritdoc />
    public virtual string ConnectionId => $"{GetType().Name}#{Params}";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="params">Connection parameters</param>
    /// <param name="loggerFactory"></param>
    protected AClient(TConnectionParams @params, ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        Params = @params;
    }

    /// <inheritdoc />
    public abstract void Dispose();

    /// <inheritdoc />
    public abstract bool Connected { get; }

    /// <inheritdoc />
    public abstract void Close();

    /// <inheritdoc />
    public abstract IDeviceStream GetStream();

    /// <inheritdoc />
    public abstract void Open();

    /// <param name="token"></param>
    /// <inheritdoc />
    public abstract Task OpenAsync(CancellationToken token);

    /// <inheritdoc />
    public abstract void Reset();

    /// <inheritdoc />
    public virtual bool Equals(IClient other) => ConnectionId.Equals(other.ConnectionId);
}