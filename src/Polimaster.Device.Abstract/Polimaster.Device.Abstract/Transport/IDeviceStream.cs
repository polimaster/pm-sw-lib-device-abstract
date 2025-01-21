using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport;

/// <summary>
/// Stream for reading/writing data to device.
/// Provide base <see cref="Read"/> and <see cref="Write"/> methods.
/// </summary>
public interface IDeviceStream<T> {
    /// <summary>
    /// Write data to device stream.
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    Task Write(T data, CancellationToken cancellationToken);

    /// <summary>
    /// Read from device stream
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns></returns>
    public Task<T> Read(CancellationToken cancellationToken);
}
