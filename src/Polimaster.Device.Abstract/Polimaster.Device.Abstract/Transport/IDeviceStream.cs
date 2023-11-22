using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;


/// <summary>
/// Stream for reading/writing data to device
/// </summary>
public interface IDeviceStream<T> {
    /// <summary>
    /// Write <see cref="T"/> buffer to device stream
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task WriteAsync(T buffer, CancellationToken cancellationToken);
    
    /// <summary>
    /// Read from device stream
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<T> ReadAsync(CancellationToken cancellationToken);
}

/// <inheritdoc />
public abstract class ADeviceStream<T> : IDeviceStream<T> {
    /// <inheritdoc />
    public abstract Task WriteAsync(T buffer, CancellationToken cancellationToken);

    /// <inheritdoc />
    public abstract Task<T> ReadAsync(CancellationToken cancellationToken);
}

/// <inheritdoc />
public abstract class StringDeviceStream : ADeviceStream<string>{}

/// <inheritdoc />
public abstract class ByteDeviceStream : ADeviceStream<byte[]>{}
