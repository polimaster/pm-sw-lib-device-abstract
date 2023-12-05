using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class AClient<T, TConnectionParams> : IClient<T> where TConnectionParams : IStringify {
    /// <summary>
    /// Logger factory
    /// </summary>
    protected ILoggerFactory? LoggerFactory { get; }

    /// <summary>
    /// Connection parameters
    /// </summary>
    protected readonly TConnectionParams Params;

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
    public abstract IDeviceStream<T> GetStream();

    /// <inheritdoc />
    public abstract void Open();

    /// <param name="token"></param>
    /// <inheritdoc />
    public abstract Task OpenAsync(CancellationToken token);

    /// <inheritdoc />
    public abstract void Reset();

    /// <inheritdoc cref="IStringify.ToString" />
    public override string ToString() => Params.ToString();
}