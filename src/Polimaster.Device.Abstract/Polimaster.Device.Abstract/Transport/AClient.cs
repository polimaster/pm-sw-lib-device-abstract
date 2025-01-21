using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class AClient<TStream, TConnectionParams> : IClient<TStream> where TConnectionParams : IFormattable {
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
    public abstract Task Open(CancellationToken token);

    /// <inheritdoc />
    public abstract TStream GetStream();

    /// <inheritdoc />
    public abstract void Reset();

    /// <inheritdoc />
    public virtual bool Equals(IClient<TStream> other) => ConnectionId.Equals(other.ConnectionId);

}