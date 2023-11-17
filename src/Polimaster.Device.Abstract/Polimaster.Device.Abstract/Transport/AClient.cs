using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <inheritdoc />
public abstract class AClient<T, TConnectionParams> : IClient<T> {
    /// <summary>
    /// Connection parameters
    /// </summary>
    protected readonly TConnectionParams ConnectionParams;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionParams">Connection parameters</param>
    protected AClient(TConnectionParams connectionParams) {
        ConnectionParams = connectionParams;
    }

    /// <inheritdoc />
    public abstract void Dispose();

    /// <inheritdoc />
    public abstract bool Connected { get; }

    /// <inheritdoc />
    public abstract void Close();

    /// <inheritdoc />
    public abstract Task<IDeviceStream<T>> GetStream();

    /// <inheritdoc />
    public abstract void Open();

    /// <inheritdoc />
    public abstract Task OpenAsync();

    /// <inheritdoc />
    public override string? ToString() {
        return ConnectionParams?.ToString();
    }
}