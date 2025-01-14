using System.Threading;
using System.Threading.Tasks;

namespace Polimaster.Device.Abstract.Transport.Stream;

/// <summary>
/// Stream for reading/writing data to device
/// </summary>
public interface IDeviceStream {
    /// <summary>
    /// Write data to device stream.
    /// </summary>
    /// <param name="data">Data to write</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="channel">Defines how and where data should be written. On example, to specific IP port or Bluetooth service.</param>
    /// <returns></returns>
    Task WriteAsync<T>(byte[] data, CancellationToken cancellationToken, T? channel = default);

    /// <summary>
    /// Read from device stream
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="channel">Defines how data should be read. On example, from specific IP port or Bluetooth service.</param>
    /// <returns></returns>
    public Task<byte[]> ReadAsync<T>(CancellationToken cancellationToken, T? channel = default);
}