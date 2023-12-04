using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class AClient<T, TConnectionParams> : IClient<T> {
    /// <summary>
    /// Logger factory
    /// </summary>
    protected ILoggerFactory? LoggerFactory { get; }

    /// <summary>
    /// Connection parameters
    /// </summary>
    protected readonly TConnectionParams ConnectionParams;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionParams">Connection parameters</param>
    /// <param name="loggerFactory"></param>
    protected AClient(TConnectionParams connectionParams, ILoggerFactory? loggerFactory) {
        LoggerFactory = loggerFactory;
        ConnectionParams = connectionParams;
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

    /// <inheritdoc />
    public abstract Task OpenAsync();

    /// <inheritdoc />
    public override string? ToString() {
        return ConnectionParams?.ToString();
    }
}